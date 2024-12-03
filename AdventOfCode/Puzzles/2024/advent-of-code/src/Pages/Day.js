import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { Solutions } from "../solutions";

const Home = () => {
	const dayNumber = useParams().day;
	let [inputText, setInputText] = useState("");
	let [solution, setSolution] = useState(null);
	let [solutionPart1, setSolutionPart1] = useState(null);
	let [solutionPart2, setSolutionPart2] = useState(null);
	let [solutionPart1Time, setSolutionPart1Time] = useState(null);
	let [solutionPart2Time, setSolutionPart2Time] = useState(null);

	useEffect(() => {
		fetch(`/Inputs/Day${dayNumber}.txt`)
			.then((res) => res.text())
			.then((text) => {
				setInputText(text);
			})
			.catch((e) => console.error(e));
		const sol = Solutions[`Day${dayNumber}`] ?? null;
		setSolution(sol);
	}, [dayNumber, solution]);

	const showSolution = async (part) => {
		if (
			solution &&
			((solution.part1 && part === 1) || (solution.part2 && part === 2))
		) {
			const startTime = performance.now();
			let result = null;
			if (part === 1) {
				result = await solution.part1(inputText);
				setSolutionPart1(result);
			} else {
				result = await solution.part2(inputText);
				setSolutionPart2(result);
			}
			const endTime = performance.now();
			console.log(`Part ${part} took ${endTime - startTime}ms`);
			if (part === 1) {
				setSolutionPart1Time(endTime - startTime);
			} else {
				setSolutionPart2Time(endTime - startTime);
			}
		} else {
			alert("No solution yet");
		}
	};

	return (
		<>
			<h1>Day {dayNumber}</h1>
			<iframe
				src={`https://adventofcode.com/2024/day/${dayNumber}`}
				title={`Day ${dayNumber}`}
				style={{ width: "100%", height: "400px" }}
			/>
			<div className="row">
				<div className="col-2">
					<h2>Input:</h2>
					<textarea
						style={{ maxHeight: "400px", height: "400px", resize: "none" }}
						readOnly
						value={inputText || "No Input Text"}
					></textarea>
				</div>
				<div className="col-10">
					<div className="row text-center">
						<div className="col-6">
							<h2>Part 1:</h2>
							<button
								onClick={async () => {
									await showSolution(1);
								}}
							>
								{solution && solution.part1
									? "Show solution part 1"
									: "No solution yet"}
							</button>
							<pre>{solutionPart1}</pre>
							{solutionPart1Time && (
								<pre>Part 1 took {solutionPart1Time}ms</pre>
							)}
						</div>
						<div className="col-6">
							<h2>Part 2:</h2>
							<button
								onClick={async () => {
									await showSolution(2);
								}}
							>
								{solution && solution.part2
									? "Show solution part 2"
									: "No solution yet"}
							</button>
							<pre>{solutionPart2}</pre>
							{solutionPart2Time && (
								<pre>Part 2 took {solutionPart2Time}ms</pre>
							)}
						</div>
					</div>
				</div>
			</div>
		</>
	);
};

export default Home;
