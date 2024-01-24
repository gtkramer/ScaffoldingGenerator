#!/usr/bin/env bash
set -e

SCRIPT_NAME="$(basename "${0}")"

usage() {
    echo """
Usage: ${SCRIPT_NAME} <input_grammar_file>

Required options:
  Input Antlr4 grammar file.  Must live in same directory as source files.

Options:
  -h, --help    Print this usage and then exit.
"""
}

if (( ${#} == 0 )); then
    usage
    exit 1
fi

while (( ${#} != 0 )); do
    case "$1" in
        -h|--help)
            usage
            exit 0
            ;;
        *)
            if [[ -z "${INPUT_GRAMMAR}" ]]; then
                INPUT_GRAMMAR="$(realpath "${1}")"
                INPUT_GRAMMAR_FILENAME="$(basename "${INPUT_GRAMMAR}")"
                INPUT_GRAMMAR_DIR="$(dirname "${INPUT_GRAMMAR}")"
                shift
            else
                echo "Unknown option: ${1}" >&2
                usage
                exit 1
            fi
            ;;
    esac
done

if [[ -z "${INPUT_GRAMMAR}" ]]; then
    echo "Error: Input grammar file is required." >&2
    usage
    exit 1
fi

cd "${INPUT_GRAMMAR_DIR}"
antlr4 -Dlanguage=CSharp -visitor -no-listener "${INPUT_GRAMMAR_FILENAME}"
find . \( -name '*.interp' -o -name '*.tokens' \) -exec rm -f {} +
