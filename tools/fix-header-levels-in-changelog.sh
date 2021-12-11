#!/bin/bash
# Workaround for buggy behavior
# Change '### [' to '## ['
echo "Running level fixer"
DIR="./"
sed -r -e 's/^#{1,3} \[/## [/' -i $DIR/CHANGELOG.md
sed -r -e 's/^\- /\* /' -i $DIR/CHANGELOG.md
