$(function () {
    $('#btnAddIngredient').click(function () {
        var index = $('.ingredient-item').length;
        var $toAdd = $('.ingredient-item-template').clone();
        $toAdd.removeClass('template ingredient-item-template');
        $toAdd.addClass('ingredient-item');
        $toAdd.find('.select-ingredient').attr('name', 'Ingredients[' + index + '].IngredientId');
        $toAdd.find('.ingredient-quantity').attr('name', 'Ingredients[' + index + '].Quantity');
        $toAdd.find('.ingredient-unit').attr('name', 'Ingredients[' + index + '].Unit');
        $toAdd.find('.select-ingredient').select2({
            ajax: {
                url: "/Ingredient/SearchIngredient",
                dataType: 'json',
                type: 'post',
                delay: 250,
                cache: true,
                data: function (params) {
                    return {
                        keyword: params.term,
                        page: params.page || 1,
                        rows: 3
                    };
                },
                processResults: function (data, params) {
                    params.page = params.page || 1;
                    $.each(data.Data, function (i, item) {
                        item.id = item.IngredientId;
                        item.text = item.Name;
                    });
                    return {
                        results: data.Data,
                        pagination: {
                            more: params.page < data.TotalPage
                        }
                    };
                }
            },
            minimumInputLength: 1,
            minimumResultsForSearch: Infinity,
            escapeMarkup: function (markup) { return markup; },
            templateResult: function (repo) {
                if (repo.loading) return repo.text;
                var markup = "<b>" + repo.Name + "</b>";
                if (repo.Description) {
                    markup += " - <span class='text-grey'>" + repo.Description + "</span>";
                }
                return markup;
            },
            theme: "bootstrap"
        });
        $toAdd.appendTo($('.ingredient-list'));
    });

    $('.btnDelIngredient').click(function () {
        this.parentElement.parentElement.remove();
    });

    $('#TagIds').select2({
        multiple: true,
        ajax: {
            url: "/Tag/SearchTag",
            type: "post",
            delay: 250,
            cache: true,
            data: function (params) {
                return {
                    keyword: params.term,
                    page: params.page || 1,
                    rows: 10
                };
            },
            processResults: function (data, params) {
                params.page = params.page || 1;
                $.each(data.Data, function (i, item) {
                    item.id = item.TagId;
                    item.text = item.Name;
                });
                return {
                    results: data.Data,
                    pagination: {
                        more: params.page < data.TotalPage
                    }
                };
            }
        },
        minimumInputLength: 1,
        escapeMarkup: function (markup) { return markup; },
        templateResult: function (repo) {
            if (repo.loading) return repo.text;
            var markup = "<b>" + repo.Name + "</b>";
            if (repo.Description) {
                markup += " - <span class='text-grey'>" + repo.Description + "</span>";
            }
            return markup;
        },
        closeOnSelect: false,
        theme: "bootstrap"
    }).on('select2:open', function () {
        $('.select2-search__field').show().focus();
    }).on('select2:close', function () {
        $('.select2-search__field').hide();
    });
    $('.select2-search__field').hide();
})