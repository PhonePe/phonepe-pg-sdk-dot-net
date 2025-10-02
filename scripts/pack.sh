#!/bin/bash
CSPROJ_FILE="src/pg-sdk-dotnet.csproj"
PACKAGE_NAME="phonepe-pg-sdk-dotnet"
VERSION="2.0.2"
OUT_DIR="../src/bin/Release"

echo "Cleaning old packages..."
rm -f ${OUT_DIR}/*.nupkg

dotnet build "$CSPROJ_FILE" -c Release

echo "Packing new version: $VERSION"
dotnet pack "$CSPROJ_FILE" -c Release \
  -p:PackageId=$PACKAGE_NAME \
  -p:Version=$VERSION \
  -o $OUT_DIR
