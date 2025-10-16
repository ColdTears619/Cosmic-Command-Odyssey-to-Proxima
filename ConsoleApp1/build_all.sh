#!/bin/bash

# Define project path (change if needed)
PROJECT_PATH="."

# Define output folder
OUTPUT_FOLDER="$PROJECT_PATH/publish"

# Create output folder
mkdir -p $OUTPUT_FOLDER

# Array of target runtimes
declare -a RUNTIMES=("win-x64" "linux-x64" "osx-x64")

# Loop through runtimes and publish
for RID in "${RUNTIMES[@]}"
do
    echo "Publishing for $RID..."
    dotnet publish $PROJECT_PATH -c Release -r $RID --self-contained true /p:PublishSingleFile=true -o "$OUTPUT_FOLDER/$RID"
done

echo "✅ All builds completed! Check the publish folder."

