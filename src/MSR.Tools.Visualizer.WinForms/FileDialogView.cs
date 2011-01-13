/*
 * MSR Tools - tools for mining software repositories
 * 
 * Copyright (C) 2010  Semyon Kirnosenko
 */

using System;
using System.Windows.Forms;

namespace MSR.Tools.Visualizer.WinForms
{
	public class FileDialogView : IFileDialogView
	{
		private string fileName;
		private SaveFileDialog saveDialog;
		private OpenFileDialog openDialog;

		public bool Open()
		{
			if (openDialog == null)
			{
				openDialog = new OpenFileDialog();
			}
			return ShowDialog(openDialog);
		}
		public bool Save()
		{
			if (saveDialog == null)
			{
				saveDialog = new SaveFileDialog();
			}
			return ShowDialog(saveDialog);
		}
		public string FileName
		{
			get { return fileName; }
			set { fileName = value; }
		}
		private bool ShowDialog(FileDialog dialog)
		{
			dialog.FileName = fileName;
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				fileName = dialog.FileName;
				return true;
			}
			return false;
		}
	}
}