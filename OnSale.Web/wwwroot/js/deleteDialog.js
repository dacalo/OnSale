(function (onSaleDeleteDialog) {
    var methods = {
        "openModal": openModal,
        "deleteItem": deleteItem
    };
    var item_to_delete;
    /*
     * Open a modal by class name or Id.
     *
     * @return string id item.
     */
    function openModal(modalName, classOrld, sourceEvent, deletePath, eventClassOrld) {
        var textEvent;
        if (classOrld) {
            textEvent = "." + modalName;
        } else {
            textEvent = "#" + modalName;
        }
        $(textEvent).click((e) => {
            item_to_delete = e.currentTarget.dataset.id;
            deleteItem(sourceEvent, deletePath, eventClassOrld)
        });
    }

    /**
     * Path to delete an item.
     *
     * @return void.
     */
    function deleteItem(sourceEvent, deletePath, eventClassOrld) {
        var textEvent;
        if (eventClassOrld) {
            textEvent = "." + sourceEvent;
        } else {
            textEvent = "#" + sourceEvent;
        }
        $(textEvent).click(function () {
            window.location.href = deletePath + item_to_delete;
        });
    }

    onSaleDeleteDialog.sc_deleteDialog = methods;
})(window);