$(function() {
    $(".hatebin-click-panel").on("click", function(event) {
        $(".hatebin-click-panel").removeClass("hatebin-selected");
        $(event.currentTarget).addClass("hatebin-selected");
        $("#SelectedItem").val($(event.currentTarget).data("item-name"));
    });
});