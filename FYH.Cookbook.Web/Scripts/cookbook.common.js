
String.prototype.replaceAll = function (search, replace) {
    var regex = new RegExp(search, 'g');
    return this.replace(regex, replace);
};
String.prototype.contains = function (str) {
    if (this.indexOf(str) >= 0) return true;
    else return false;
};
Array.prototype.contains = function (item) {
    var isIn = false;
    $.each(this, function (index, value) {
        if (item === value) isIn = true;
    });
    return isIn;
};

$.extend({
    CbAjax: function (opts) {
        opts = $.extend({
            url: undefined,
            type: 'POST',
            data: undefined,
            dataType: 'json',
            contentType: 'application/x-www-form-urlencoded',
            async: true,
            cache: false,
            onBeforeSend: undefined,    // XMLHttpRequest request, Object ajaxObject
            onSuccess: undefined,       // Object responseData, String successString = success, XMLHttpResponse response
            onError: undefined,         // XMLHttpResponse response, String errorString = error, String statusText
            onComplete: undefined       // XMLHttpResponse response, String completeString
        }, opts);

        $.ajax({
            url: opts.url,
            type: opts.type,
            data: opts.data,
            dataType: opts.dataType,
            contentType: opts.contentType,
            async: opts.async,
            cache: opts.cache,
            beforeSend: opts.onBeforeSend,
            success: opts.onSuccess,
            error: function (response, errorString, statusText) {
                console.log('response: ' + JSON.stringify(response));
                console.log('errorString: ' + errorString);
                console.log('statusText: ' + statusText);

                opts.onError && opts.onError(response, errorString, statusText);
            },
            complete: opts.onComplete
        });
    }
});

$.fn.extend({
    CbSerialize: function () {
        var data = {},
            serializeArray = [],
            multiLevelArray = [];

        var temp = [];
        $.each(this.serializeArray(), function (i, v) {
            if (!temp.contains(v.name)) {
                temp.push(v.name);
                serializeArray.push(v);
            }
        });
        serializeArray = serializeArray.sort(function (a, b) {
            return a.name.localeCompare(b.name);
        });

        $.each(serializeArray, function (i, v) {
            var names = v.name.split('.'),
                nameList = [];

            $.each(names, function (i, v) {
                if (v.contains('[')) {
                    nameList.push(v.substring(0, v.indexOf('[')));
                    nameList.push(v.substring(v.indexOf('['), v.indexOf(']') + 1));
                } else {
                    nameList.push(v);
                }
            });

            multiLevelArray.push({ names: nameList, value: v.value || '' });
        });

        if (multiLevelArray && multiLevelArray.length) {
            var _convertName = function (array, deep) {
                var items = [],
                        currentItem = undefined;
                $.each(array, function (i, v) {
                    if (deep < v.names.length) {
                        var name = v.names[deep];
                        if (currentItem && currentItem.name === name) {
                            currentItem.value.push(v);
                        } else {
                            currentItem = { name: name, value: [v] };
                            items.push(currentItem);
                        }
                    }
                });

                if (items && items.length) {
                    $.each(items, function (i, v) {
                        if (v.name.contains('[')) {
                            $.each(v.value, function (vi, vv) {
                                v.value[vi].names[deep] = '[' + i + ']';
                            });
                        }

                        _convertName(v.value, deep + 1);
                    });
                }
            };
            _convertName(multiLevelArray, 0);

            $.each(multiLevelArray, function (i, v) {
                var name = v.names.join('.').replaceAll('\\.\\[', '[');
                if (data[name] === undefined) {
                    data[name] = v.value;
                }
            });
        }

        return data;
    },
    CbBindValidator: function() {
        var $forms = this.parents()
            .addBack()
            .filter('form')
            .add(this.find('form'))
            .has('[data-val=true]');

        if ($forms && $forms.length) {
            $forms.removeData('validator');
            $.validator && $.validator.unobtrusive.parse($forms);
        }

        return $(this);
    }
});

/*
 callback: 
 opts:
   opts.size: [large, middle, small(default)]
 */
alert = function (message, callback, opts) {
    var $modal = initModal(
        'Notice',
        '<div>' + message + '</div>',
        {
            size: opts && opts.size ? opts.size : 'small',
            btns: [
                '<button type="button" class="btn btn-danger" data-dismiss="modal" data-btntype="ok" style="width:70px;">Ok</button>'
            ]
        }
    );
    $modal.modal({
        backdrop: 'static',
        show: true
    }).on('hidden.bs.modal', function (e) {
        $modal.remove();
        callback && callback();
    });
    return $modal;
};

/*
 callback: Boolean result
 opts:
   opts.size: [large, middle, small(default)]
 */
confirm = function (message, callback, opts) {
    var $modal = initModal(
        'Notice',
        '<div>' + message + '</div>',
        {
            size: opts && opts.size ? opts.size : 'small',
            close: false,
            btns: [
                '<button type="button" class="btn btn-danger" data-dismiss="modal" data-btntype="ok" onclick="this.dataset.marked=true" style="width:70px;">Ok</button>',
                '<button type="button" class="btn btn-default" data-dismiss="modal" data-btntype="cancel" onclick="this.dataset.marked=true" style="width:70px;">Cancel</button>'
            ]
        }
    );
    $modal.modal({
        backdrop: 'static',
        show: true
    }).on('hidden.bs.modal', function (e) {
        if (callback) {
            var $btn = $modal.find('button[data-marked=true]');
            if (!$btn || !$btn.length || $btn.data('btntype') === 'cancel') {
                callback(false);
            } else if ($btn.data('btntype') === 'ok') {
                callback(true);
            }
        }
        $modal.remove();
    });
    return $modal;
};

/*
 opts:
   opts.size: [large, middle(default), small]
 loadedCallBack: Boolean result, Object modalObject
 closedCallBack: Boolean result, Object modalObject
 */
var loadUrl = function (title, url, opts, loadedCallBack, closedCallBack) {
    $.CbAjax({
        url: url,
        type: 'GET',
        dataType: 'text',
        onBeforeSend: function () {
            showLoadingLayer();
        },
        onSuccess: function (responseData, successString, response) {
            hideLoadingLayer();
            var $modal = initModal(
                title,
                responseData,
                {
                    size: opts && opts.size ? opts.size : 'middle',
                    close: true
                }
            );
            $modal.CbBindValidator();
            $modal.modal({
                backdrop: 'static',
                show: true
            }).on('shown.bs.modal', function (e) {
                loadedCallBack && loadedCallBack(true, $modal);
            }).on('hidden.bs.modal', function (e) {
                $(this).remove();
                closedCallBack && closedCallBack(true, $modal);
            });
        },
        onError: function (response, errorString, statusText) {
            hideLoadingLayer();
            var $modal = initModal(
                title,
                '<p>' + errorString + ': ' + statusText + '</p>',
                {
                    size: opts && opts.size ? opts.size : 'middle',
                    close: true
                }
            );
            $modal.modal({
                backdrop: 'static',
                show: true
            }).on('shown.bs.modal', function (e) {
                loadedCallBack && loadedCallBack(false, $modal);
            }).on('hidden.bs.modal', function (e) {
                $(this).remove();
                closedCallBack && closedCallBack(false, $modal);
            });
        }
    });
};

/*
 title: title
 content: content support html
 opts:
   opts.size: [large, middle(default), small]
   opts.close: [true(default), false]
 */
var showModal = function (title, content, opts) {
    initModal(
        title,
        '<div>' + content + '</div>',
        {
            size: opts && opts.size ? opts.size : 'middle',
            close: opts && opts.close != undefined ? opts.close : true,
        }
    ).modal({
        backdrop: 'static',
        show: true
    });
};

/*
 title: content
 content: content support html
 opts:
   opts.size: [large, middle(default), small]
   opts.close: [true, false(default)]
   opts.btns: [btn1, btn2...]
 */
var initModal = function (title, content, opts) {
    var sizeClass = undefined,
        btns = undefined;
    if (opts) {
        if (opts.size === 'large') {
            sizeClass = ' modal-lg';
        } else if (opts.size === 'middle') {
            sizeClass = '';
        } else if (opts.size === 'small') {
            sizeClass = ' modal-sm';
        } else {
            sizeClass = '';
        }

        if (opts.btns) {
            btns = opts.btns.join('');
        }
    }

    var $modalContent = $('<div>').addClass('modal-content'),
        hasTitle = title !== undefined && title !== null && title !== '',
        hasColse = opts && (opts.close === true || opts.close === 'true');
    if (hasTitle || hasColse) {
        if (!hasTitle) {
            title = '&nbsp;';
        }

        var $header = $('<div>').addClass('modal-header');
        if (hasColse) {
            $header.append(
                $('<button>').attr({ type: 'button', 'data-dismiss': 'modal', 'aria-label': 'Close' }).addClass('close').append(
                    $('<span>').attr({ 'aria-hidden': 'true' }).html('&times;')
                )
            );
        }
        $header.append(
            $('<h5>').addClass('modal-title').html(title)
        );
        $modalContent.append($header);
    }
    $modalContent.append(
        $('<div>').addClass('modal-body').append(content)
    );
    if (btns) {
        $modalContent.append(
            $('<div>').addClass('modal-footer').append(btns)
        );
    }

    return $('<div>').addClass('modal fade dj-modal').append(
        $('<div>').addClass('modal-dialog' + sizeClass).append(
            $modalContent
        )
    );
};

var showLoadingLayer = function () {
    hideLoadingLayer();

    $('body').append(
        $('<div>').addClass('cb-masklayer').append(
            $('<div>').addClass('cb-maskmsg').append(
                $('<span>').addClass('cb-refresh text-danger mr10')
            ).append(arguments[0] ? arguments[0] : 'loading...')
        )
    );
};

var hideLoadingLayer = function () {
    $('body div.cb-masklayer').remove();
};