import { DIRECTIONS, inputParser, UTILS } from "../solutionHelpers";

const Day12 = {
	walkTheCrop: (plots, cropType, position, checked, cropsArr = []) => {
		const directions = [
			DIRECTIONS.up,
			DIRECTIONS.down,
			DIRECTIONS.left,
			DIRECTIONS.right,
		];
		const { x, y } = position;
		if (UTILS.checkIsInBounds(x, y, plots[0].length, plots.length)) {
			if (!checked.includes(`${x},${y}`) && plots[y][x] === cropType) {
				checked.push(`${x},${y}`);
				cropsArr.push(position);
			}
		}
		directions.forEach((dir) => {
			const nextPos = DIRECTIONS.getNextPos(dir, x, y, 1);
			if (
				UTILS.checkIsInBounds(
					nextPos.x,
					nextPos.y,
					plots[0].length,
					plots.length
				) &&
				!checked.includes(`${nextPos.x},${nextPos.y}`) &&
				plots[nextPos.y][nextPos.x] === cropType
			) {
				checked.push(`${nextPos.x},${nextPos.y}`);
				cropsArr.push(nextPos);
				Day12.walkTheCrop(plots, cropType, nextPos, checked, cropsArr);
			}
		});
		return cropsArr.length === 0 ? [position] : cropsArr;
	},
	setup: (input) => {
		const plots = inputParser.parseAsMultiDimentionArray(input, "");
		// add space for fencing
		const widthWithFencing = plots[0].length * 2 + 1;
		for (let y = plots.length - 1; y >= 0; y--) {
			for (let x = plots[y].length - 1; x >= 0; x--) {
				plots[y].splice(x, 0, " ");
			}
			plots[y].push(" ");
			plots.splice(
				y,
				0,
				Array.from({ length: widthWithFencing }, () => " ")
			);
		}
		plots.push(Array.from({ length: plots[0].length }, () => " "));
		const crops = {}; // { [key: string]: Array<{x: number, y: number}> }
		for (let y = 1; y < plots.length; y += 2) {
			for (let x = 1; x < plots[y].length; x += 2) {
				const cropType = plots[y][x];
				crops[cropType] = crops[cropType] || [];
				crops[cropType].push({ x, y });
				if (y === 1 || plots[y - 2][x] !== cropType) {
					plots[y - 1][x] = "-"; // fence
				}
				if (x === 1 || plots[y][x - 2] !== cropType) {
					plots[y][x - 1] = "|"; // fence
				}
				if (y === plots.length - 2 || plots[y + 2][x] !== cropType) {
					plots[y + 1][x] = "-"; // fence
				}
				if (x === plots[y].length - 2 || plots[y][x + 2] !== cropType) {
					plots[y][x + 1] = "|"; // fence
				}
			}
		}
		return { plots, crops };
	},
	getContiguousCropSections(plots, crops) {
		const contiguousCropSections = [];
		Object.keys(crops).forEach((cropType) => {
			const crop = crops[cropType];
			let checkedPlots = [];
			crop.forEach((plot) => {
				if (!checkedPlots.includes(`${plot.x},${plot.y}`)) {
					contiguousCropSections.push({
						cropType,
						crops: Day12.walkTheCrop(plots, cropType, plot, checkedPlots),
					});
				}
			});
		});
		return contiguousCropSections;
	},
	part1: async (input) => {
		const { plots, crops } = Day12.setup(input);
		let fenceCost = 0;
		const contiguousCropSections = Day12.getContiguousCropSections(
			plots,
			crops
		);
		contiguousCropSections.forEach((cropSection) => {
			let fenceCount = 0;
			cropSection.crops.forEach((plot) => {
				if (plots[plot.y - 1][plot.x] === "-") {
					fenceCount++;
				}
				if (plots[plot.y + 1][plot.x] === "-") {
					fenceCount++;
				}
				if (plots[plot.y][plot.x - 1] === "|") {
					fenceCount++;
				}
				if (plots[plot.y][plot.x + 1] === "|") {
					fenceCount++;
				}
			});
			fenceCost += fenceCount * cropSection.crops.length;
			// console.log(
			// 	`Area for ${cropSection.cropType}`,
			// 	cropSection.crops.length,
			// 	`fenceCount for ${cropSection.cropType}`,
			// 	fenceCount,
			// 	`Fence Cost for ${cropSection.cropType}`,
			// 	fenceCount * cropSection.crops.length
			// );
		});
		return fenceCost;
	},
	walkCropParimeter: (
		plots,
		crops,
		startPositionOverride = null,
		isRecurse = false,
		recursedCheckArr = null
	) => {
		// get upperleft most crop
		const startPos = startPositionOverride
			? { ...startPositionOverride }
			: [...crops].sort((a, b) => {
					if (a.y === b.y) {
						return a.x - b.x;
					}
					return a.y - b.y;
			  })[0];
		let curPos = { ...startPos };
		let direction = DIRECTIONS.right;
		const fenceArr = [];
		let curLeftFence = null;
		while (
			fenceArr.length < 2 ||
			fenceArr[0].pos.x !== curLeftFence.pos.x ||
			fenceArr[0].pos.y !== curLeftFence.pos.y
		) {
			const wallFenceDirection = DIRECTIONS.getDirectionAfterTurn(
				direction,
				"L"
			);
			const wallFencePos = DIRECTIONS.getNextPos(
				wallFenceDirection,
				curPos.x,
				curPos.y
			);
			// if there is no fence to the left, move to the left
			curLeftFence = {
				type: plots[wallFencePos.y][wallFencePos.x],
				pos: wallFencePos,
			};
			if (curLeftFence.type === " ") {
				direction = wallFenceDirection;
				curPos = DIRECTIONS.getNextPos(direction, curPos.x, curPos.y, 1);
				continue;
			} else {
				fenceArr.push(curLeftFence);
				recursedCheckArr?.push(curLeftFence);
			}
			let frontFencePos = DIRECTIONS.getNextPos(direction, curPos.x, curPos.y);
			if (plots[frontFencePos.y][frontFencePos.x] === " ") {
				curPos = DIRECTIONS.getNextPos(direction, curPos.x, curPos.y, 1);
			} else {
				if (
					plots[frontFencePos.y][frontFencePos.x] === "|" ||
					plots[frontFencePos.y][frontFencePos.x] === "-"
				) {
					direction = DIRECTIONS.getDirectionAfterTurn(direction, "R");
					frontFencePos = DIRECTIONS.getNextPos(direction, curPos.x, curPos.y);
				}
			}
		}
		let fenceSides = 0;
		if (!isRecurse) {
			const recursedCheck = [...fenceArr];
			crops.forEach((crop) => {
				// find lower fence of encapsulated crop to recurse over
				if (
					plots[crop.y - 1][crop.x] === "-" &&
					!recursedCheck.find(
						(f) => f.pos.x === crop.x && f.pos.y === crop.y - 1
					)
				) {
					fenceSides += Day12.walkCropParimeter(
						plots,
						crops,
						crop,
						true,
						recursedCheck
					);
				}
			});
		}
		for (let i = 0; i < fenceArr.length - 1; i++) {
			const fence = fenceArr[i];
			const nextFence = fenceArr[i + 1];
			if (fence.type !== nextFence.type) {
				fenceSides++;
			}
		}
		return fenceSides;
	},
	part2: async (input) => {
		const { plots, crops } = Day12.setup(input);
		let fenceCost = 0;
		const contiguousCropSections = Day12.getContiguousCropSections(
			plots,
			crops
		);
		contiguousCropSections.forEach((cropSection) => {
			let fenceSides = Day12.walkCropParimeter(plots, cropSection.crops);
			fenceCost += fenceSides * cropSection.crops.length;
			console.log(
				`Area for ${cropSection.cropType}: ${
					cropSection.crops.length
				}, fenceSides for ${
					cropSection.cropType
				}: ${fenceSides}, Fence Cost for ${cropSection.cropType}: ${
					fenceSides * cropSection.crops.length
				}}`
			);
		});
		Day12.printPlots(plots);
		return fenceCost;
	},
	printPlots: (plots) => {
		console.log(
			plots
				.map((y, yi, arr) =>
					y
						.map((x, xi) => {
							if (x === " ") {
								if (xi === 0 || arr[yi][xi - 1] === "-") {
									return "+";
								}
								if (xi === y.length - 1 || arr[yi][xi + 1] === "-") {
									return "+";
								}
								if (yi === 0 || arr[yi - 1][xi] === "|") {
									return "+";
								}
								if (yi === arr.length - 1 || arr[yi + 1][xi] === "|") {
									return "+";
								}
							}
							return x;
						})
						.join("")
				)
				.join("\n")
		);
	},
};

export default Day12;
