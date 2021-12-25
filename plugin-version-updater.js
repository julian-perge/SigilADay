module.exports.readVersion = function (contents) {
  return GetNumbersFromContents(contents);
}

const regex = /\d{1,3}.\d{1,3}.\d{1,3}/;

module.exports.writeVersion = function (contents, version) {
  contents = contents.replace(regex, version);
  return contents;
}

function GetNumbersFromContents(contents) {
  const number = contents.match(new RegExp(regex));
  console.log("Found number! = " + number);
  return number;
}
