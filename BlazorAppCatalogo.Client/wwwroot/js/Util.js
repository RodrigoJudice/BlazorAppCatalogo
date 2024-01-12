function focusById(elementId) {

    console.debug("focusById");
    console.debug("elementId", elementId);

    var element = document.getElementById(elementId);
    if (element) {
        element.focus();
    }
}