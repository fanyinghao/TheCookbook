$(function() {
    $("#btnDel").on({
        click: function () {
            confirm("Are you sure to delete this recipe?", function(e) {
                if (e) {
                    window.location = $("#btnDel").attr('href');
                }
            });
            return false;
        }
    });
});