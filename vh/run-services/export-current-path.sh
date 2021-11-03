#!/bin/bash

# It exports the directory where the all .sh are located to work.
FULL_PATH_FILE=$(realpath $0)
export VH_CURRENT_SCRIPT_PATH=$(dirname "$FULL_PATH_FILE")
