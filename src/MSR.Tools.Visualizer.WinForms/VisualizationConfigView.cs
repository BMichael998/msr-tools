/*
 * MSR Tools - tools for mining software repositories
 * 
 * Copyright (C) 2011  Semyon Kirnosenko
 */

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MSR.Tools.Visualizer.WinForms
{
	public class ConfigView : Form, IConfigView
	{
		private PropertyGrid properties;
		
		public ConfigView()
		{
			properties = new PropertyGrid();
			properties.Parent = this;
			properties.PropertySort = PropertySort.NoSort;
			properties.Dock = DockStyle.Fill;
		}
		public void Add(object obj)
		{
			properties.SelectedObject = obj;
		}
		public new bool ShowDialog()
		{
			base.ShowDialog();
			return true;
		}
	}
}