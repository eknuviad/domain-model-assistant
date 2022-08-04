#!/usr/bin/env node

const exec = require('child_process');
const fs = require('fs');

const unityFrameworkJsFile = "unity-webgl-output/Build/unity-webgl-output.framework.js";
const unityFrameworkGzFile = `${unityFrameworkJsFile}.gz`;

console.log("Deminifying Unity WebGL Framework JS file...");

// Unzip GZ file
exec.execSync(`gunzip -kf ${unityFrameworkGzFile}`);

// Read Unity Framework JS file
fs.readFile(unityFrameworkJsFile, (err, data) => {
  if (err) {
    console.error(err);
    return;
  }
  fs.writeFile(unityFrameworkJsFile, deminify(data.toString()), (err) => {
    if (err) {
      console.error(err);
      return;
    }
    exec.execSync(`gzip -kf ${unityFrameworkJsFile}`);
    console.log(`Deminified ${unityFrameworkJsFile}`);
  });
});

function deminify(jsSource) {
  return (jsSource
      .replaceAll(";if(", ";\n  if (")
      .replaceAll("}if(", "}\n  if (")
      .replaceAll(";for(", ";\n  for (")
      .replaceAll("}for(", "}\n  for (")
      .replaceAll(";var ", ";\n  var ")
      .replaceAll("}var ", "}\n  var ")
  );
}

