/*
 * MSR Tools - tools for mining software repositories
 * 
 * Copyright (C) 2010  Semyon Kirnosenko
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using MSR.Data;
using MSR.Data.Entities;
using MSR.Data.Entities.DSL.Selection;
using MSR.Data.Entities.DSL.Selection.Metrics;
using MSR.Data.Persistent;
using MSR.Models;

namespace MSR.Tools.Debugger
{
	public class DebuggingTool : Tool
	{
		public DebuggingTool(string configFile)
			: base(configFile)
		{
		}
		public void Predict(string previousReleaseRevision, string releaseRevision)
		{
			using (ConsoleTimeLogger.Start("prediction"))
			using (var s = data.OpenSession())
			{
				PostReleaseDefectFilePredictionEvaluation evaluator = new PostReleaseDefectFilePredictionEvaluation(s);
				Dictionary<string, IPostReleaseDefectFilePrediction> predictors = new Dictionary<string, IPostReleaseDefectFilePrediction>()
				{
					{ "random", new RandomPostReleaseDefectFilePrediction(s) },
					{ "max loc", new MaxLocPostReleaseDefectFilePrediction(s) },
					{ "max added loc", new MaxAddedLocPostReleaseDefectFilePrediction(s) },
					{ "max touch", new MaxTouchCountPostReleaseDefectFilePrediction(s) },
					{ "dd based", new DefectDensityBasedDefectFilePrediction(s) },
					{ "dcd based", new DefectCodeDensityBasedDefectFilePrediction(s) }
				};

				evaluator.PostReleasePeriod = 30 * 6;
				evaluator.PreviousReleaseRevision = previousReleaseRevision;
				evaluator.ReleaseRevision = releaseRevision;
				evaluator.FileSelector = fe => fe.InDirectory("/trunk");

				foreach (var predictor in predictors)
				{
					Console.WriteLine(predictor.Key + ":");
					EvaluationResult result = evaluator.Evaluate(predictor.Value);
					Console.WriteLine(result);
				}
			}
		}
		public void QueryUnderProfiler()
		{
			PersistentDataStoreProfiler prof = new PersistentDataStoreProfiler(data);

			using (var s = data.OpenSession())
			{
			}

			Console.WriteLine("{0}", prof);
		}
		public void LocStat()
		{
			using (var s = data.OpenSession())
			{
				RepositorySelectionExpression selectionDSL = new RepositorySelectionExpression(s);

				var code = selectionDSL
					.Files()
						.Exist()
					.Modifications()
						.InFiles()
					.CodeBlocks()
						.InModifications().Fixed();

				Console.WriteLine(
					"LOC+ {0}",
					code.Added().CalculateLOC()
				);
				Console.WriteLine(
					"LOC- {0}",
					code.Deleted().CalculateLOC()
				);
				Console.WriteLine(
					"LOCc {0}",
					code.Added().CalculateLOC() + code.Added().ModifiedBy().CalculateLOC()
				);
				Console.WriteLine(
					"LOCm- {0}",
					code.Added().ModifiedBy().CalculateLOC()
				);
				Console.WriteLine(
					"LOCmf- {0}",
					code.Added().ModifiedBy().InBugFixes().CalculateLOC()
				);
				Console.WriteLine("DD {0}", code.CalculateTraditionalDefectDensity());
				Console.WriteLine("DCD {0}", code.CalculateDefectCodeDensity());
			}
		}
		
		public void Blame(string path, string revision)
		{
			var blame = scmDataNoCache.Blame(revision, path);
			
			SmartDictionary<string, IEnumerable<int>> output =
				new SmartDictionary<string,IEnumerable<int>>((x) => new List<int>());
			for (int i = 1; i <= blame.Count; i++)
			{
				(output[blame[i]] as List<int>).Add(i);
			}
			foreach (var rev in output.OrderBy(x => x.Key))
			{
				Console.WriteLine("{0} {1}", rev.Key, rev.Value.Count());
			}
		}
	}
}
