mergeInto(LibraryManager.library, {
    IsMobileDevice: function() {
        var userAgent = navigator.userAgent || navigator.vendor || window.opera;
        return /android|iphone|ipad|iPod|windows phone/i.test(userAgent) ? 1 : 0;
    }
});
