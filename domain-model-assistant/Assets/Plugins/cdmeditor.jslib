mergeInto(LibraryManager.library, {
  SetCursorToAddMode: function () {
    document.getElementById("unity-canvas").style.cursor = "copy"; // (+) symbol
  },
  ResetCursor: function () {
    document.getElementById("unity-canvas").style.cursor = "default";
  }
});
