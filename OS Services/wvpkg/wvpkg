#!/bin/bash

# Variables
VERBOSE=false
SOURCE_DIR=""
OUTPUT_DIR=""
HELP=false
INSTALL=false
DELETE=false
PACKAGE=false
# help
show_usage() {
    cat << EOF
Usage: $0 [OPTIONS]

WaveOS Package Manager

OPTIONS:
    -v, --verbose       Enable verbose output
    -i, --install       Install a package
    -d, --delete        Delete the .wvpkg file after installation
    -p, --package       Package a folder as a WaveOS app
    -s, --source DIR    Specify source directory
    -o, --output DIR    Specify output directory
    -h, --help          Show this help message

EOF
}

# Function for verbose logging
log_verbose() {
    if [ "$VERBOSE" = true ]; then
        echo "[VERBOSE] $1"
    fi
}

# Parse command line arguments
while [[ $# -gt 0 ]]; do
    case $1 in
        -v|--verbose)
            VERBOSE=true
            shift
            ;;
        -i|--install)
            INSTALL=true
            PACKAGE=false
            shift
            ;;
        -d|--delete)
            DELETE=true
            shift
            ;;
        -p|--package)
            PACKAGE=true
            INSTALL=false
            shift
            ;;    
        -s|--source)
            SOURCE_DIR="$2"
            shift 2
            ;;
        -o|--output)
            OUTPUT_DIR="$2"
            shift 2
            ;;
        -h|--help)
            HELP=true
            shift
            ;;
        -*|--*)
            echo "Unknown option $1"
            show_usage
            exit 1
            ;;
        *)
            echo "Unknown positional argument: $1"
            show_usage
            exit 1
            ;;
    esac
done

# Show help if requested
if [ "$HELP" = true ]; then
    show_usage
    exit 0
fi

# Making sure only one building mode is selected
if [ "$INSTALL" = true ] && [ "$PACKAGE" = true ]; then
    echo "Error: You cannot use both -i (install) and -p (package) at the same time."
    show_usage
    exit 1
fi
# Making sure at least one building mode is selected
if [ "$INSTALL" = false ] && [ "$PACKAGE" = false ]; then
    echo "Error: You must specify either -i (install) or -p (package)."
    show_usage
    exit 1
fi

# Validate required arguments
if [ -z "$SOURCE_DIR" ]; then
    echo "Error: Source directory/file is required. Use -s or --source to specify."
    show_usage
    exit 1
fi

if [ -z "$OUTPUT_DIR" ]; then
    echo "Error: Output directory/file is required. Use -o or --output to specify."
    show_usage
    exit 1
fi

# Validate source exists
if [ "$INSTALL" = true ]; then
    # For installation, source should be a file (.wvpkg)
    if [ ! -f "$SOURCE_DIR" ]; then
        echo "Error: Source file '$SOURCE_DIR' does not exist."
        exit 1
    fi
else
    # For packaging, source should be a directory
    if [ ! -d "$SOURCE_DIR" ]; then
        echo "Error: Source directory '$SOURCE_DIR' does not exist."
        exit 1
    fi
fi

# Create output directory if needed (only for installation)
if [ "$INSTALL" = true ] && [ ! -d "$OUTPUT_DIR" ]; then
    log_verbose "Creating output directory: $OUTPUT_DIR"
    mkdir -p "$OUTPUT_DIR"
fi
# Validate that the source directory is not the same as the output directory
if [ "$SOURCE_DIR" = "$OUTPUT_DIR" ]; then
    echo "Error: Source directory/file and output directory/file cannot be the same."
    exit 1
fi
# Validate that output directory is a .wvpkg file if packaging
if [ "$PACKAGE" = true ] && [[ ! "$OUTPUT_DIR" =~ \.wvpkg$ ]]; then
    echo "Error: When packaging, the output directory/file must be a .wvpkg file."
    exit 1
fi
# Validate that source directory/file is a WaveOS app directory if installing
if [ "$INSTALL" = true ] && [[ ! "$SOURCE_DIR" =~ \.wvpkg$ ]]; then
    echo "Error: When installing, the source directory/file must be a valid WaveOS app .wvpkg file."
    exit 1
fi


# Main script logic
if [ "$INSTALL" = true ]; then
    log_verbose "Starting .wvpkg installation..."
    log_verbose "Source file: $SOURCE_DIR"
    log_verbose "Output directory: $OUTPUT_DIR"
    echo "Installing app from '$SOURCE_DIR' to '$OUTPUT_DIR'..."
else
    log_verbose "Starting .wvpkg packaging..."
    log_verbose "Source directory: $SOURCE_DIR"
    log_verbose "Output file: $OUTPUT_DIR"
    echo "Packaging directory '$SOURCE_DIR' into WaveOS app at '$OUTPUT_DIR'..."
fi


if [ "$INSTALL" = true ]; then
    # Installing apps
    if [ -z "$OUTPUT_DIR" ]; then
        OUTPUT_DIR="~/Applications"
    fi
    log_verbose "Uncompressing app file"
    unzip -o "$SOURCE_DIR" -d "$OUTPUT_DIR"
    if [ $? -ne 0 ]; then
        echo "Error: Failed to install app from '$SOURCE_DIR' to '$OUTPUT_DIR'."
        exit 1
    fi
    # Clean up
    if [ "$DELETE" = true ]; then
        log_verbose "Deleting source file after installation."
        rm -f "$SOURCE_DIR"
        if [ $? -ne 0 ]; then
            echo "Error: Failed to delete source file '$SOURCE_DIR'."
            exit 1
        fi
    fi
else
    # Packaging apps
    log_verbose "Creating .wvpkg file"
    (cd "$SOURCE_DIR" && zip -r "$OUTPUT_DIR" . -x "*.git*" -x "*.DS_Store")
    if [ $? -ne 0 ]; then
        echo "Error: Failed to package directory '$SOURCE_DIR' into '$OUTPUT_DIR'."
        exit 1
    fi
fi

if [ "$INSTALL" = true ]; then
    echo "Installation complete. App installed to '$OUTPUT_DIR'."
else
    echo "Packaging complete. App packaged to '$OUTPUT_DIR'."
fi