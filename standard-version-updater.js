// standard-version-updater.js
const stringifyPackage = require('stringify-package')
const detectIndent = require('detect-indent')
const detectNewline = require('detect-newline')

module.exports.readVersion = function (contents) {
  return JSON.parse(contents).version_number;
}

module.exports.writeVersion = function (contents, version) {
  const json = JSON.parse(contents)
  let indent = detectIndent(contents).indent
  let newline = detectNewline(contents)
  json.version_number = version
  return stringifyPackage(json, indent, newline)
}
