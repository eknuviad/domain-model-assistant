mergeInto(LibraryManager.library, {
  // Functions to perform HTTP requests on WebGL
  GetRequest: async function (url) {
    const response = await fetch(url);
    const json = await response.json();
    const jsonStr = JSON.stringify(json);
    var size = lengthBytesUTF8(jsonStr) + 1;
    var ptr = _malloc(size);
    stringToUTF8(jsonStr, ptr, size);
    return ptr;
  },

  // UI-related functions
  SetCursorToAddMode: function () {
    document.getElementById("unity-canvas").style.cursor = "copy"; // (+) symbol
  },
  ResetCursor: function () {
    document.getElementById("unity-canvas").style.cursor = "default";
  },
});
