mergeInto(LibraryManager.library, {
  // Functions to perform HTTP requests on WebGL
  GetRequest: async function (url) {
    const urlStr = UTF8ToString(url);
    const response = await fetch(urlStr);
    const json = await response.json();
    const jsonStr = JSON.stringify(json);
    const size = lengthBytesUTF8(jsonStr) + 1;
    var ptr = _malloc(size);
    stringToUTF8(jsonStr, ptr, size);
    console.log(`_GetRequest(${urlStr}: ${typeof urlStr}) returning ${UTF8ToString(ptr)}`);
    return UTF8ToString(ptr);
  },

  // UI-related functions
  SetCursorToAddMode: function () {
    document.getElementById("unity-canvas").style.cursor = "copy"; // (+) symbol
  },
  ResetCursor: function () {
    document.getElementById("unity-canvas").style.cursor = "default";
  },
});
