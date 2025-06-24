window.bootstrapModalHelper = {
    show: function(modalSelector) {
        var modalElement = document.querySelector(modalSelector);
        var modal = bootstrap.Modal.getOrCreateInstance(modalElement);
        modal.show();
    },
    hide: function(modalSelector) {
        var modalElement = document.querySelector(modalSelector);
        var modal = bootstrap.Modal.getOrCreateInstance(modalElement);
        modal.hide();
    }
};