#!/bin/bash

# Name:    PersonatorConsumerCloudAPI
# Purpose: Execute the PersonatorConsumerCloudAPI program

######################### Constants ##########################

RED='\033[0;31m' #RED
NC='\033[0m' # No Color

######################### Parameters ##########################

addressline1=""
city=""
state=""
postal=""
country=""
license=""

while [ $# -gt 0 ] ; do
  case $1 in
    --addressline1)  
        if [ -z "$2" ] || [[ $2 == -* ]];
        then
            printf "${RED}Error: Missing an argument for parameter \'addressline1\'.${NC}\n"  
            exit 1
        fi 

        addressline1="$2"
        shift
        ;;
    --city) 
        if [ -z "$2" ] || [[ $2 == -* ]];
        then
            printf "${RED}Error: Missing an argument for parameter \'city\'.${NC}\n"  
            exit 1
        fi 

        city="$2"
        shift
        ;;
    --state)         
        if [ -z "$2" ] || [[ $2 == -* ]];
        then
            printf "${RED}Error: Missing an argument for parameter \'state\'.${NC}\n"  
            exit 1
        fi 
        
        state="$2"
        shift
        ;;
    --postal) 
        if [ -z "$2" ] || [[ $2 == -* ]];
        then
            printf "${RED}Error: Missing an argument for parameter \'postal\'.${NC}\n"  
            exit 1
        fi 

        postal="$2"
        shift
        ;;
    --country) 
        if [ -z "$2" ] || [[ $2 == -* ]];
        then
            printf "${RED}Error: Missing an argument for parameter \'country\'.${NC}\n"  
            exit 1
        fi 

        country="$2"
        shift
        ;;
    --license) 
        if [ -z "$2" ] || [[ $2 == -* ]];
        then
            printf "${RED}Error: Missing an argument for parameter \'license\'.${NC}\n"  
            exit 1
        fi 

        license="$2"
        shift 
        ;;
  esac
  shift
done

# Use the location of the .sh file
# Modify this if you want to use
CurrentPath="$(pwd)"
ProjectPath="$CurrentPath/PersonatorConsumerDotnet"
BuildPath="$ProjectPath/Build"

if [ ! -d "$BuildPath" ];
then
    mkdir "$BuildPath"
fi

########################## Main ############################
printf "\n==================== Melissa Personator Consumer Cloud API =====================\n"

# Get license (either from parameters or user input)
if [ -z "$license" ];
then
  printf "Please enter your license string: "
  read license
fi

# Check for License from Environment Variables 
if [ -z "$license" ];
then
  license=`echo $MD_LICENSE` 
fi

if [ -z "$license" ];
then
  printf "\nLicense String is invalid!\n"
  exit 1
fi

# Start program
# Build project
printf "\n================================= BUILD PROJECT ================================\n"

dotnet publish -f="net8.0" -c Release -o $BuildPath PersonatorConsumerDotnet/PersonatorConsumerDotnet.csproj

# Run project
if [ -z "$addressline1" ] && [ -z "$city" ] && [ -z "$state" ] && [ -z "$postal" ] && [ -z "$country" ];
then
    dotnet "$BuildPath"/PersonatorConsumerDotnet.dll --license $license 
else
    dotnet "$BuildPath"/PersonatorConsumerDotnet.dll --license $license --addressline1 "$addressline1" --city "$city" --state "$state" --postal "$postal" --country "$country"
fi

