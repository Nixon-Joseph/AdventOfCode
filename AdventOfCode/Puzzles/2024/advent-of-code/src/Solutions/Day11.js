const Day11 = {
	setup: (input) => input.split(" ").filter((x) => x !== ""),
	processBlinks: (stones, numBlinks) => {
		let blinks = numBlinks;
		let stoneCounts = {};
		stones.forEach((stone) => {
			stoneCounts[stone] = 1;
		});
		while (blinks > 0) {
			let newCounts = {};
			// eslint-disable-next-line no-loop-func
			Object.keys(stoneCounts).forEach((stoneKey) => {
				const stone = stoneKey;
				const count = stoneCounts[stoneKey];

				var newStones = Day11.processBlink(stone);
				newStones.forEach((newStone) => {
					if (newCounts[newStone] === undefined) {
						newCounts[newStone] = count;
					} else {
						newCounts[newStone] += count;
					}
				});
			});

			stoneCounts = newCounts;
			blinks--;
		}

		return Object.keys(stoneCounts).reduce(
			(acc, stoneKey) => acc + stoneCounts[stoneKey],
			0
		);
	},
	processBlink: (stone) => {
		let output = [];
		if (stone === "0") {
			output.push("1");
		} else if (stone.length % 2 === 0) {
			const left = stone.substring(0, stone.length / 2);
			const right = stone.substring(stone.length / 2);
			output.push(left);
			output.push(Number(right).toString());
		} else {
			const product = (Number(stone) * 2024).toString();
			output.push(product);
		}
		return output;
	},
	part1: async (input) => {
		const stones = Day11.setup(input);
		return Day11.processBlinks([...stones], 25);
	},
	part2: async (input) => {
		const stones = Day11.setup(input);
		return Day11.processBlinks([...stones], 75);
	},
};

export default Day11;
