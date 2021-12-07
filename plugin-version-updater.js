module.exports.readVersion = function (contents) {
  return GetNumbersFromContents(contents);
}

module.exports.writeVersion = function (contents, version) {
  contents = contents.replace(/\d.\d.\d/, version);
  return contents;
}

function GetNumbersFromContents(contents) {
  const number = contents.match(new RegExp(/\d.\d.\d/));
  console.log("Found number! = " + number);
  return number;
}
