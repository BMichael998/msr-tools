/*
 * MSR Tools - tools for mining software repositories
 * 
 * Copyright (C) 2010  Semyon Kirnosenko
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using ZedGraph;

namespace MSR.Tools.Visualizer
{
	public class VisualizerPresenter
	{
		private VisualizerModel model;
		private IVisualizerView view;
		private IPresenterFactory presenters;

		public VisualizerPresenter(VisualizerModel model, IVisualizerView view, IPresenterFactory presenters)
		{
			this.model = model;
			this.view = view;
			this.presenters = presenters;
			
			CreateMainMenu();
		}
		public void Show()
		{
			view.ShowView();
		}
		public void CreateMainMenu()
		{
			view.MainMenu.AddCommand("Visualizer");
			view.MainMenu.AddCommand("Open config...", "Visualizer").OnClick += i =>
			{
				string fileName = presenters.FileDialog().OpenFile(null);
				if (fileName != null)
				{
					try
					{
						model.OpenConfig(fileName);
						view.Title = fileName;
					}
					catch (Exception e)
					{
						presenters.MessageDialog().Error(e.Message);
					}
				}
			};
			view.MainMenu.AddCommand("Scale");
			view.MainMenu.AddCommand("log x", "Scale").OnClick += i =>
			{
				i.Checked = ! i.Checked;
				view.Graph.XAxisLogScale = i.Checked;
			};
			view.MainMenu.AddCommand("log y", "Scale").OnClick += i =>
			{
				i.Checked = ! i.Checked;
				view.Graph.YAxisLogScale = i.Checked;
			};
			view.MainMenu.AddCommand("Visualizations");
			view.MainMenu.AddCommand("bugs", "Visualizations").OnClick += i =>
			{
				model.Visualize(i.Name, view.Graph);
			};
		}
	}
}