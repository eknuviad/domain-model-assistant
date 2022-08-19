mergeInto(LibraryManager.library, {
  // Functions to perform HTTP requests on WebGL
  HttpRequest: function (verb, url, headers, data) {
    const verbAllCaps = UTF8ToString(verb).toUpperCase();
    const urlStr = UTF8ToString(url);
    const headersStr = headers ? UTF8ToString(headers) : "";
    const headersObj = headersStr ? JSON.parse(headersStr) : {};
    const dataStr = UTF8ToString(data);

    console.log(`Sending HttpRequest(\n  verb=${verbAllCaps},\n  url=${urlStr},\n  headers=${JSON.stringify(headersObj)
        },\n  data=${dataStr || ""}\n)`);

    const request = new XMLHttpRequest();
    // The `false` indicates that the request is synchronous, which is deprecated and should be replaced once
    // Unity WebGL supports asynchronous requests
    request.open(verbAllCaps, urlStr, false);
    // URL in form http[s]://server[:port]/username/classdiagram/cdmname
    const creatingCdm = verbAllCaps === "PUT" && /https?:\/\/[^/]+\/[^/]+\/classdiagram\/[^/]+\/?$/.test(urlStr);
    if (!("Content-Type" in headersObj) && !creatingCdm) {
      request.setRequestHeader("Content-Type", "application/json");
    }
    Object.entries(headersObj).forEach(([name, value]) => request.setRequestHeader(name, value));
    try {
      request.send(dataStr);
    } catch (e) {
      console.error(`${verbAllCaps} request failed to send with error ${e}\nResponse headers: ${
          JSON.stringify(request.getAllResponseHeaders())}`);
      return null;
    }
    if (request.status >= 300) {
      console.error(`${verbAllCaps} request status is error ${request.status}`);
    }
    const result = _ConvertToUnityString(request.responseText);
    console.log(`HttpRequest() returning ${UTF8ToString(result)}`);
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
