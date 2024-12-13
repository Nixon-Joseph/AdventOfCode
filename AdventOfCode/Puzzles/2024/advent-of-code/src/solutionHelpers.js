export const inputParser = {
	parseAsMultiDimentionArray: (
		input,
		splitChar = " ",
		removeEmpties = true,
		returnAsNumbers = false
	) => {
		return input.split("\r\n").map((line) =>
			line
				.split(splitChar)
				.filter((item) => (removeEmpties ? item !== "" : true))
				.map((x) => {
					const val = x.trim();
					return returnAsNumbers ? Number(val) : val;
				})
		);
	},
	parseAsLeftRightArrays: (
		input,
		splitChar = " ",
		removeEmpties = true,
		returnAsNumbers = false
	) => {
		const returnArrs = { left: [], right: [] };
		input.split("\r\n").forEach((line) => {
			const lineContents = line
				.split(splitChar)
				.filter((item) => (removeEmpties ? item !== "" : true))
				.map((x) => x.trim());
			if (lineContents.length === 2) {
				returnArrs.left.push(
					returnAsNumbers ? Number(lineContents[0]) : lineContents[0]
				);
				returnArrs.right.push(
					returnAsNumbers ? Number(lineContents[1]) : lineContents[1]
				);
			}
		});
		return returnArrs;
	},
	parseAsLineArray: (input, splitChar = "\r\n", removeEmpties = true) => {
		return input
			.split(splitChar)
			.filter((item) => (removeEmpties ? item !== "" : true))
			.map((x) => x.trim());
	},
};

export const UTILS = {
	checkIsInBounds: (x, y, width, height) =>
		y >= 0 && x >= 0 && y < height && x < width,
};

export const DIRECTIONS = {
	up: 0,
	down: 1,
	left: 2,
	right: 3,
	upLeft: 4,
	upRight: 5,
	downLeft: 6,
	downRight: 7,
	getNextPos: (direction, x, y, additionalDistance = 0) => {
		const distance = 1 + additionalDistance;
		switch (direction) {
			case DIRECTIONS.up:
				return { x, y: y - distance };
			case DIRECTIONS.down:
				return { x, y: y + distance };
			case DIRECTIONS.left:
				return { x: x - distance, y };
			case DIRECTIONS.right:
				return { x: x + distance, y };
			case DIRECTIONS.upLeft:
				return { x: x - distance, y: y - distance };
			case DIRECTIONS.upRight:
				return { x: x + distance, y: y - distance };
			case DIRECTIONS.downLeft:
				return { x: x - distance, y: y + distance };
			case DIRECTIONS.downRight:
				return { x: x + distance, y: y + distance };
			default:
				throw new Error("Invalid direction");
		}
	},
	getDirectionAfterTurn: (currentDirection, turn) => {
		switch (currentDirection) {
			case DIRECTIONS.up:
				return turn === "L" ? DIRECTIONS.left : DIRECTIONS.right;
			case DIRECTIONS.down:
				return turn === "L" ? DIRECTIONS.right : DIRECTIONS.left;
			case DIRECTIONS.left:
				return turn === "L" ? DIRECTIONS.down : DIRECTIONS.up;
			case DIRECTIONS.right:
				return turn === "L" ? DIRECTIONS.up : DIRECTIONS.down;
			default:
				throw new Error("Invalid direction");
		}
	},
};