echo "Building waveOS"
if [ -f "build-config.json" ]; then
    echo "Reading build configuration from JSON..."
    APPS_OUTPUT_DIR=$(jq -r '.os_apps_output_directory' build-config.json)
    echo "Apps output directory: $APPS_OUTPUT_DIR"
    TARGET_ARCH=$(jq -r '.target_architecture' build-config.json)
    echo "Target architecture: $TARGET_ARCH"
else
    echo "build-config.json not found. Exiting."
    exit 1
fi
if [ "$(uname -m)" = "$TARGET_ARCH" ]; then
    echo "Building for $TARGET_ARCH architecture..."
else
    echo "Unsupported architecture. Must be $TARGET_ARCH. Exiting."
    exit 1
fi

echo "Building OS Apps"
jq -r '.os_apps[] | @base64' build-config.json | while read encoded_app; do
    app_data=$(echo "$encoded_app" | base64 -d)
    source_folder_path=$(echo "$app_data" | jq -r '.Folder_Path')
    app_type=$(echo "$app_data" | jq -r '.Type')
    name=$(echo "$app_data" | jq -r '.Name')

    echo "Processing app: $name"
    echo "Type: $app_type"
    echo "Path: $source_folder_path"

    if [ "$app_type" = "flutter" ]; then
        echo "Building Flutter app..."
        previous_dir=$(pwd)
        cd "$source_folder_path"
        flutter build linux --release 
        if [ ! -d "$APPS_OUTPUT_DIR/$name" ]; then
            mkdir -p "$APPS_OUTPUT_DIR/$name"
        fi
        mv build/linux/arm64/release/bundle "$APPS_OUTPUT_DIR/$name"
        cd "$previous_dir"
    elif [ "$app_type" = "csharp" ]; then
        echo "Building C# app..."
        previous_dir=$(pwd)
        cd "$source_folder_path"
        dotnet publish -c Release -r linux-arm64
        if [ ! -d "$APPS_OUTPUT_DIR/$name" ]; then
            mkdir -p "$APPS_OUTPUT_DIR/$name"
        fi
        mv bin/Release/net9.0/linux-arm64/publish "$APPS_OUTPUT_DIR/$name"
        cd "$previous_dir"
    fi
done
echo "All apps built."
echo "Building OS Services..."
jq -r '.os_services[] | @base64' build-config.json | while read encoded_service; do
    service_data=$(echo "$encoded_service" | base64 -d)
    source_folder_path=$(echo "$service_data" | jq -r '.Folder_Path')
    service_type=$(echo "$service_data" | jq -r '.Type')
    name=$(echo "$service_data" | jq -r '.Name')
    output_folder=$(echo "$service_data" | jq -r '.Output_Path')

    echo "Processing service: $name"
    echo "Type: $service_type"
    echo "Path: $source_folder_path"
    echo "Output folder: $output_folder"

    if [ "$service_type" = "flutter" ]; then
        echo "Building Flutter service..."
        previous_dir=$(pwd)
        cd "$source_folder_path"
        flutter build linux --release 
        if [ ! -d "$output_folder" ]; then
            mkdir -p "$output_folder"
        fi
        mv build/linux/arm64/release/bundle "$output_folder"
        cd "$previous_dir"
    elif [ "$service_type" = "csharp" ]; then
        echo "Building C# service..."
        previous_dir=$(pwd)
        cd "$source_folder_path"
        dotnet publish -c Release -r linux-arm64
        if [ ! -d "$output_folder" ]; then
            mkdir -p "$output_folder"
        fi
        mv bin/Release/net9.0/linux-arm64/publish "$output_folder"
        cd "$previous_dir"
    fi
done
echo "All services built."
echo "All subsystems built. Check files now and see if they are correct."
echo "Do you want to build waveOS now? (y/n)"
read build_waveos
if [ "$build_waveos" = "y" ]; then
    echo "Building waveOS..."
    cd WaveOS
    ./build.sh -c cm4-slim.cfg -D ./WaveOS -o WaveOS/cm4.options
else
    echo "Skipping waveOS build."
fi
echo "Build process completed."