﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Diagnostics;
using ICSharpCode.Core;
using ICSharpCode.PythonBinding;
using NUnit.Framework;
using PythonBinding.Tests.Utils;

namespace PythonBinding.Tests.Testing
{
	[TestFixture]
	public class PythonConsoleApplicationTestFixture
	{
		PythonConsoleApplication app;
		PythonAddInOptions options;
		
		[SetUp]
		public void Init()
		{
			options = new PythonAddInOptions(new Properties());
			options.PythonFileName = @"C:\IronPython\ipy.exe";
			app = new PythonConsoleApplication(options);
		}
		
		[Test]
		public void FileNameIsPythonFileNameFromAddInOptions()
		{
			string expectedFileName = @"C:\IronPython\ipy.exe";
			Assert.AreEqual(expectedFileName, app.FileName);
		}
		
		[Test]
		public void GetArgumentsReturnsDebugOptionWhenDebugIsTrue()
		{
			app.Debug = true;
			string expectedCommandLine = "-X:Debug";
			
			Assert.AreEqual(expectedCommandLine, app.GetArguments());
		}
		
		[Test]
		public void GetArgumentsReturnsQuotedPythonScriptFileName()
		{
			app.PythonScriptFileName = @"d:\projects\my ipy\test.py";
			string expectedCommandLine = "\"d:\\projects\\my ipy\\test.py\"";
			
			Assert.AreEqual(expectedCommandLine, app.GetArguments());
		}
		
		[Test]
		public void GetArgumentsReturnsQuotedPythonScriptFileNameAndItsCommandLineArguments()
		{
			app.Debug = true;
			app.PythonScriptFileName = @"d:\projects\my ipy\test.py";
			app.PythonScriptCommandLineArguments = "@responseFile.txt -def";
			string expectedCommandLine =
				"-X:Debug \"d:\\projects\\my ipy\\test.py\" @responseFile.txt -def";
			
			Assert.AreEqual(expectedCommandLine, app.GetArguments());
		}
		
		[Test]
		public void GetProcessStartInfoHasFileNameThatEqualsIronPythonConsoleApplicationExeFileName()
		{
			ProcessStartInfo startInfo = app.GetProcessStartInfo();
			string expectedFileName = @"C:\IronPython\ipy.exe";
			
			Assert.AreEqual(expectedFileName, startInfo.FileName);
		}
		
		[Test]
		public void GetProcessStartInfoHasDebugFlagSetInArguments()
		{
			app.Debug = true;
			ProcessStartInfo startInfo = app.GetProcessStartInfo();
			string expectedCommandLine = "-X:Debug";
			
			Assert.AreEqual(expectedCommandLine, startInfo.Arguments);
		}
		
		[Test]
		public void GetProcessStartInfoHasWorkingDirectoryIfSet()
		{
			app.WorkingDirectory = @"d:\temp";
			ProcessStartInfo startInfo = app.GetProcessStartInfo();
			Assert.AreEqual(@"d:\temp", startInfo.WorkingDirectory);
		}
		
		[Test]
		public void ChangingOptionsPythonFileNameChangesProcessStartInfoFileName()
		{
			options.PythonFileName = @"d:\temp\test\ipy.exe";
			ProcessStartInfo startInfo = app.GetProcessStartInfo();
			
			Assert.AreEqual(@"d:\temp\test\ipy.exe", startInfo.FileName);
		}
	}
}