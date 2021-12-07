// standard-version-updater.js
const stringifyPackage = require('stringify-package')
const detectIndent = require('detect-indent')
const detectNewline = require('detect-newline')

module.exports.readVersion = function (contents) {
  return GetNumbersFromContents(contents);
}

module.exports.writeVersion = function (contents, version) {
  contents = contents.replace(/\d.\d.\d/, version);
  return contents;
}

function GetNumbersFromContents(contents) {
  const versionNumberMatch = "\d.\d.\d";
  const number = contents.toString().match(new RegExp(versionNumberMatch));
  console.log("Found number! =" + number);
  return number;
}
