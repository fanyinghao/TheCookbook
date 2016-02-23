var downloadPdf = function () {
    var doc = new jsPDF();
    var specialElementHandlers = {
        '#editor': function (element, renderer) {
            return true;
        }
    };

    doc.fromHTML($(".div-to-preview").html() + $("footer").html(), 15, 15, {
        'width': 170,
        'elementHandlers': specialElementHandlers
    });

    doc.save($("#Name").val() + ".pdf");
}

$(function () {
    $("#btnDel").on({
        click: function () {
            confirm("Are you sure to delete this recipe?", function (e) {
                if (e) {
                    window.location = $("#btnDel").attr('href');
                }
            });
            return false;
        }
    });

    $("#btnPreview").on({
        click: function () {
            var btns = '<div class="row">' +
                '<div class="col-lg-2"><button class="btn btn-lg btn-danger" id="btnPrint" onclick="window.print();">Print</button></div>' +
                '<div class="col-lg-2"><button class="btn btn-lg btn-danger" id="btnDownload" onclick="downloadPdf();">Download</button></div>' +
                '<div class="col-lg-2"><button class="btn btn-lg btn-default" data-dismiss="modal">Cancel</button></div>' +
                '</div></div>';
            showModal("Print Preview", "<div class='print-hide'>" + $(".div-to-preview").html().replace(/col-lg-8/g, "col-lg-12") + btns + "</div>");
        }
    });
});