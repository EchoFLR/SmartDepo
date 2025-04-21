#!/bin/bash

WATCH_DIR="/home/flawlessruby/Fullstack/Trams/SmartDepo"

TEST_PROJECT="/home/flawlessruby/Fullstack/Trams/SmartDepo.Tests"

echo "Watching for changes in $WATCH_DIR..."
echo "Running tests in $TEST_PROJECT..."

inotifywait -r -m -e modify,create,delete --exclude '(\.swp$|\.tmp$|\.git/)' $WATCH_DIR |
while read -r directory events filename; do
    if [[ "$filename" =~ \.(cs|json|csproj)$ ]]; then
        echo "Change detected in $directory$filename. Running tests..."
        dotnet build $WATCH_DIR
        dotnet test $TEST_PROJECT
    fi
done