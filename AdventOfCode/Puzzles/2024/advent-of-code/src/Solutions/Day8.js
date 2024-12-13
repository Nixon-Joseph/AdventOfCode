import { DIRECTIONS, inputParser, UTILS } from "../solutionHelpers";

const Day8 = {
	determineDiagnoalAndDiffInterval: (pos1, pos2) => {
		const xDiff = Math.abs(pos2.x - pos1.x);
		const yDiff = Math.abs(pos2.y - pos1.y);
		// detect which diagonal the antenna is on based on the difference in x and y
		// then place antinodes on the same diagonal equedisant from the other antenna
		let diagnoal = "";
		if (pos2.x === pos1.x) {
			if (pos2.y > pos1.y) {
				diagnoal = DIRECTIONS.down;
			} else {
				diagnoal = DIRECTIONS.up;
			}
		} else if (pos2.y === pos1.y) {
			if (pos2.x > pos1.x) {
				diagnoal = DIRECTIONS.right;
			} else {
				diagnoal = DIRECTIONS.left;
			}
		} else if (pos2.x > pos1.x && pos2.y > pos1.y) {
			diagnoal = DIRECTIONS.downRight;
		} else if (pos2.x < pos1.x && pos2.y > pos1.y) {
			diagnoal = DIRECTIONS.downLeft;
		} else if (pos2.x > pos1.x && pos2.y < pos1.y) {
			diagnoal = DIRECTIONS.upRight;
		} else if (pos2.x < pos1.x && pos2.y < pos1.y) {
			diagnoal = DIRECTIONS.upLeft;
		}
		return { diagnoal, xDiff, yDiff };
	},
	getAntinodePosFuncs: (diagnoal, xDiff, yDiff) => {
		let primaryDirFunc, oppositeDirFunc;
		switch (diagnoal) {
			case DIRECTIONS.up:
				primaryDirFunc = (_x, _y) => ({ x: _x, y: _y - yDiff });
				oppositeDirFunc = (_x, _y) => ({ x: _x, y: _y + yDiff });
				break;
			case DIRECTIONS.down:
				primaryDirFunc = (_x, _y) => ({ x: _x, y: _y + yDiff });
				oppositeDirFunc = (_x, _y) => ({ x: _x, y: _y - yDiff });
				break;
			case DIRECTIONS.left:
				primaryDirFunc = (_x, _y) => ({ x: _x - xDiff, y: _y });
				oppositeDirFunc = (_x, _y) => ({ x: _x + xDiff, y: _y });
				break;
			case DIRECTIONS.right:
				primaryDirFunc = (_x, _y) => ({ x: _x + xDiff, y: _y });
				oppositeDirFunc = (_x, _y) => ({ x: _x - xDiff, y: _y });
				break;
			case DIRECTIONS.downRight:
				primaryDirFunc = (_x, _y) => ({ x: _x + xDiff, y: _y + yDiff });
				oppositeDirFunc = (_x, _y) => ({
					x: _x - xDiff,
					y: _y - yDiff,
				});
				break;
			case DIRECTIONS.downLeft:
				primaryDirFunc = (_x, _y) => ({ x: _x - xDiff, y: _y + yDiff });
				oppositeDirFunc = (_x, _y) => ({
					x: _x + xDiff,
					y: _y - yDiff,
				});
				break;
			case DIRECTIONS.upRight:
				primaryDirFunc = (_x, _y) => ({ x: _x + xDiff, y: _y - yDiff });
				oppositeDirFunc = (_x, _y) => ({
					x: _x - xDiff,
					y: _y + yDiff,
				});
				break;
			case DIRECTIONS.upLeft:
				primaryDirFunc = (_x, _y) => ({ x: _x - xDiff, y: _y - yDiff });
				oppositeDirFunc = (_x, _y) => ({
					x: _x + xDiff,
					y: _y + yDiff,
				});
				break;
			default:
				throw new Error("Invalid direction");
		}
		return { primaryDirFunc, oppositeDirFunc };
	},
	part1: async (input) => {
		var grid = inputParser.parseAsMultiDimentionArray(input, "");
		const antenia = {}; // { [key: string]: Array<{ x: number, y: number}> }
		const antinodes = []; // Array<string>
		grid.forEach((row, y) => {
			row.forEach((col, x) => {
				if (col !== ".") {
					if (antenia[col] === undefined) {
						antenia[col] = [];
					}
					antenia[col].forEach((antennaPos) => {
						const { diagnoal, xDiff, yDiff } =
							Day8.determineDiagnoalAndDiffInterval({ x, y }, antennaPos);
						const nodes = [];

						const { primaryDirFunc, oppositeDirFunc } =
							Day8.getAntinodePosFuncs(diagnoal, xDiff, yDiff);
						nodes.push(primaryDirFunc(antennaPos.x, antennaPos.y));
						nodes.push(oppositeDirFunc(x, y));

						nodes.forEach((antinode) => {
							if (
								UTILS.checkIsInBounds(
									antinode.x,
									antinode.y,
									row.length,
									grid.length
								)
							) {
								const antinodeKey = `${antinode.x},${antinode.y}`;
								if (!antinodes.includes(antinodeKey)) {
									antinodes.push(antinodeKey);
								}
							}
						});
					});
					antenia[col].push({ x, y });
				}
			});
		});
		return antinodes.length;
	},
	part2: async (input) => {
		var grid = inputParser.parseAsMultiDimentionArray(input, "");
		const antenia = {}; // { [key: string]: Array<{ x: number, y: number}> }
		const antinodes = []; // Array<string>
		grid.forEach((row, y) => {
			row.forEach((col, x) => {
				if (col !== ".") {
					if (antenia[col] === undefined) {
						antenia[col] = [];
					}

					antenia[col].forEach((antennaPos) => {
						const { diagnoal, xDiff, yDiff } =
							Day8.determineDiagnoalAndDiffInterval({ x, y }, antennaPos);
						const _antinodes = [antennaPos, { x, y }];

						const addResinantAntinodes = (pos, resinanceFunc) => {
							let lastPos = { ...pos };
							while (
								UTILS.checkIsInBounds(
									lastPos.x,
									lastPos.y,
									row.length,
									grid.length
								)
							) {
								_antinodes.push({ ...lastPos });
								lastPos = resinanceFunc(lastPos.x, lastPos.y);
							}
						};
						const { primaryDirFunc, oppositeDirFunc } =
							Day8.getAntinodePosFuncs(diagnoal, xDiff, yDiff);
						addResinantAntinodes(antennaPos, primaryDirFunc);
						addResinantAntinodes({ x, y }, oppositeDirFunc);

						_antinodes.forEach((antinode) => {
							const antinodeKey = `${antinode.x},${antinode.y}`;
							if (!antinodes.includes(antinodeKey)) {
								antinodes.push(antinodeKey);
							}
						});
					});
					antenia[col].push({ x, y });
				}
			});
		});
		return antinodes.length;
	},
};

export default Day8;
