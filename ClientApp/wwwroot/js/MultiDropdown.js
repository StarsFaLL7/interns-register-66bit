var MultiDropdown = {};
MultiDropdown.InitMultiDropdown = function (pDotNetReference, guid) {
    let $eventSelect = $("#" + guid.toString());
    $eventSelect.on("select2:select", function (evt) {
        pDotNetReference.invokeMethodAsync('SelectItemInternal', evt.params.data.id);
        console.log("select2:select", evt.params.data.id);
    });
    $eventSelect.on("select2:unselect", function (evt) {
        pDotNetReference.invokeMethodAsync('UnselectItemInternal', evt.params.data.id);
        console.log("select2:unselect", evt.params.data.id);
    });
};

MultiDropdown.Clear = function (guid) {
    let $eventSelect = $("#" + guid.toString());
    $eventSelect.val(null).trigger('change');
};

MultiDropdown.SelectItem = function (multiDdId, internalItemId) {
    let $eventSelect = $("#" + multiDdId.toString());
    $eventSelect.val(internalItemId.toString());
    $eventSelect.trigger('change'); // Notify any JS components that the value changed
}

MultiDropdown.SelectItemsRange = function (multiDdId, internalItemIdArr) {
    let $eventSelect = $("#" + multiDdId.toString());
    $eventSelect.val(internalItemIdArr);
    $eventSelect.trigger('change'); // Notify any JS components that the value changed
}