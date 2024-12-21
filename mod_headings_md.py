import os
import argparse
import re


def modify_headings(file_path, operation, start_line=None, end_line=None):
    """Modify headings in a markdown file."""
    with open(file_path, "r", encoding="utf-8") as file:
        lines = []
        for line in file:
            lines.append(line)

    # Apply modification within the specified line range
    start_line = start_line - 1 if start_line else 0
    end_line = end_line if end_line else len(lines)

    for i in range(start_line, end_line):
        if lines[i].startswith("#"):
            if operation == "add":
                lines[i] = "#" + lines[i]
            elif operation == "remove":
                lines[i] = re.sub(r"^#(#+ )", r"\1", lines[i])

    with open(file_path, "w", encoding="utf-8") as file:
        file.writelines(lines)


def main():
    parser = argparse.ArgumentParser(
        description="Modify markdown headings by adding or removing #."
    )
    parser.add_argument(
        "operation",
        choices=["add", "remove"],
        help="Operation to perform: 'add' to add # or 'remove' to remove #.",
    )
    parser.add_argument(
        "--from",
        dest="from_line",
        type=int,
        default=None,
        help="Start line (inclusive) to apply the operation.",
    )
    parser.add_argument(
        "--to",
        dest="to_line",
        type=int,
        default=None,
        help="End line (inclusive) to apply the operation.",
    )

    args = parser.parse_args()

    # Iterate over all .md files in the current directory
    for file_name in os.listdir():
        if file_name.endswith(".md"):
            print(f"Processing file: {file_name}")
            modify_headings(file_name, args.operation, args.from_line, args.to_line)


if __name__ == "__main__":
    main()
