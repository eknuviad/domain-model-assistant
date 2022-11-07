#!/usr/bin/env node

const exec = require('child_process');
const fs = require('fs');

const beautify = require('js-beautify').js; // from Beautifier.io

const unityFrameworkJsFile = "unity-webgl-output/Build/unity-webgl-output.framework.js";
const unityFrameworkGzFile = `${unityFrameworkJsFile}.gz`;

const beautifyOptions = {
  indent_size: 2,
  wrap_line_length: 120,
  end_with_newline: true,
};

console.log("Deminifying Unity WebGL Framework JS file...");

// Unzip GZ file
exec.execSync(`gunzip -kf ${unityFrameworkGzFile}`);

// Read Unity Framework JS file, deminify it, write it back to the file, and rezip it
fs.readFile(unityFrameworkJsFile, (err, data) => {
  if (err) {
    console.error(err);
    return;
  }
  fs.writeFile(unityFrameworkJsFile, deminify(data.toString()), err => {
    if (err) {
      console.error(err);
      return;
    }
    exec.execSync(`gzip -kf ${unityFrameworkJsFile}`);
    console.log(`Deminified ${unityFrameworkJsFile}`);
  });
});

/**
 * Deminify the input JS source code.
 * 
 * @param {string} jsSource 
 * @returns the deminified JS source
 */
function deminify(jsSource) {
  // break up source into multiple lines for faster processing
  jsSource = (jsSource
    .replaceAll(";if(", ";\n  if (")
    .replaceAll("}if(", "}\n  if (")
    .replaceAll(";for(", ";\n  for (")
    .replaceAll("}for(", "}\n  for (")
    .replaceAll(";var ", ";\n  var ")
    .replaceAll("}var ", "}\n  var "));
  return beautify(jsSource, beautifyOptions);
}

