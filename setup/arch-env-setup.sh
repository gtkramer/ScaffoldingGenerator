#!/bin/bash
SCRIPT_DIR="$(dirname "$(realpath "$0")")"
aurman -S CLI11 sdl boost antlr4 antlr4-runtime meson gtest glade
sudo cp "$SCRIPT_DIR/antlr4-runtime.pc" /usr/share/pkgconfig
