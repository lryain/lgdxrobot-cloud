#!/bin/bash

sleep 1s

## Generate Certificates
cd Certs
. generate-certs.sh
cd ..

## Initialise Data
dotnet LGDXRobotCloud.Data.dll --initialiseData "true" --email "email@example.com" --fullName "Full Name" --userName "admin" --password "123456" --seedData "true"