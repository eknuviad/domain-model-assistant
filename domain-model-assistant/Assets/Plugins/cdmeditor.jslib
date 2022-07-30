mergeInto(LibraryManager.library, {
  // Functions to perform HTTP requests on WebGL
  HttpRequest: function (verb, url, headers, data) {
    const verbAllCaps = UTF8ToString(verb).toUpperCase();
    const urlStr = UTF8ToString(url);
    const headersArray = JSON.parse(UTF8ToString(headers));
    const dataStr = UTF8ToString(data);
    const request = new XMLHttpRequest();
    // The `false` indicates that the request is synchronous, which is deprecated and should be replaced once
    // Unity WebGL supports asynchronous requests
    request.open(verbAllCaps, urlStr, false);
    for (var i = 0; i < headersArray.length; i++) {
      request.setRequestHeader(UTF8ToString(headers[i].name), UTF8ToString(headers[i].value));
    }
    request.send(dataStr);
    if (request.status >= 300) {
      console.error(`${verbAllCaps} request failed with error ${request.status}`);
    }
    const result = _ConvertToUnityString(request.responseText);
    console.log(`_Request(\n  verb=${verbAllCaps},\n  url=${urlStr},\n  headers=${JSON.stringify(headersArray)},\n` +
        `  data=${dataStr}\n) => ${UTF8ToString(result)}`);
    return result;
  },

  ConvertToUnityString: function (o) {
    var s = "";
    if (o === null || o === undefined) {
      s = "null";
    } else if (typeof o === "object") {
      s = JSON.stringify(o);
    } else {
      s = o.toString(); // handles numbers, booleans, etc
    }
    const size = lengthBytesUTF8(s) + 1;
    var ptr = _malloc(size);
    stringToUTF8(s, ptr, size);
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
