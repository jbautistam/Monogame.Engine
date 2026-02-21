string? folder = GetArgument("folder");
string? contentFile = GetArgument("file");

if (string.IsNullOrWhiteSpace(folder))
	Console.WriteLine("You must enter de folder with your content. Argument: -folder");
else if (string.IsNullOrWhiteSpace(contentFile))
	Console.WriteLine("You must enter de filename for your content. Argument: -file");
else if (!Directory.Exists(folder))
	Console.WriteLine($"Can't find the folder {folder}");
else
{
	string content = PrepareMgcbFile(folder);

		// Escribe el archivo
		File.WriteAllText(Path.Combine(folder, contentFile), content);
		// Muestra el contenido en la consola
		Console.WriteLine();
		Console.WriteLine(new string('=', 80));
		Console.WriteLine();
		Console.WriteLine(content);
		Console.WriteLine();
		Console.WriteLine(new string('=', 80));
		Console.WriteLine();
}

// Obtiene un argumento de la lista
string? GetArgument(string name)
{
	// Recorre los argumentos de la lista
	for (int index = 0; index < args.Length - 1; index++)
		if (args[index].Equals(name, StringComparison.CurrentCultureIgnoreCase) ||
				args[index].Equals($"-{name}", StringComparison.CurrentCultureIgnoreCase) ||
				args[index].Equals($"-{name}", StringComparison.CurrentCultureIgnoreCase))
			return args[index + 1];
	// Si ha llegado hasta aquí es porque no ha encontrado nada
	return null;
}

// Prepara el archivo MGCB para todos los directorios de los archivos
string PrepareMgcbFile(string folderBase)
{
	string result = """
					#----------------------------- Global Properties ----------------------------#

					/outputDir:bin/$(Platform)
					/intermediateDir:obj/$(Platform)
					/platform:DesktopGL
					/config:
					/profile:Reach
					/compress:False

					#-------------------------------- References --------------------------------#


					#---------------------------------- Content ---------------------------------#

					""";

		// Añade los arhcivos
		result += PrepareMgcbForFiles(folderBase, folderBase);
		// Devuelve el resultado
		return result;
}

// Prepara el archivo MGCB para todos los directorios de los archivos
string PrepareMgcbForFiles(string folderBase, string actualFolder)
{
	string result = string.Empty;

		if (!actualFolder.EndsWith("/bin", StringComparison.CurrentCultureIgnoreCase) &&
			!actualFolder.EndsWith("/obj", StringComparison.CurrentCultureIgnoreCase) &&
			!actualFolder.EndsWith("\\bin", StringComparison.CurrentCultureIgnoreCase) &&
			!actualFolder.EndsWith("\\obj", StringComparison.CurrentCultureIgnoreCase))
		{
			string childFolder = GetChildFolder(folderBase, actualFolder);

				// Prepara el contenido de los archivos
				foreach (string file in Directory.GetFiles(actualFolder))
					result += PrepareFile(childFolder, Path.GetFileName(file));
				// Obtiene el contenido de los directorios hijo
				foreach (string directory in Directory.GetDirectories(actualFolder))
					result += PrepareMgcbForFiles(folderBase, directory);
		}
		// Devuelve el resultado
		return result;
}

// Obtiene la carpeta relativa
string GetChildFolder(string folderBase, string actualFolder)
{
	if (folderBase.Length < actualFolder.Length)
		return actualFolder.Substring(folderBase.Length);
	else
		return string.Empty;
}

// Prepara el archivo
string PrepareFile(string folder, string fileName)
{
	string fileResult = Path.Combine(folder, fileName).Replace("\\", "/");

		// Prepara los archivos
		if (IsExtension(fileName, "png", "bmp", "jpg", "gif"))
			return PrepareImageFile(fileResult) + Environment.NewLine;
		else if (IsExtension(fileName, "xml", "txt", "json", "tsx", "tmx"))
			return PrepareCopyFile(fileResult) + Environment.NewLine;
		else if (IsExtension(fileName, "wav"))
			return PrepareSoundEffectWavFile(fileResult) + Environment.NewLine;
		else if (IsExtension(fileName, "mp3"))
			return PrepareSongMp3File(fileResult) + Environment.NewLine;
		else if (IsExtension(fileName, "spritefont"))
			return PrepareSpriteFontFile(fileResult) + Environment.NewLine;
		//else if (IsExtension(fileName, "fx"))
		//	return PrepareShader(fileResult) + Environment.NewLine;
		else
			return string.Empty;
}

// Comprueba si un método tiene una extensión
bool IsExtension(string fileName, params string [] extensions)
{
	// Comprueba la extensión del archivo con la lista de extensiones
	foreach (string extension in extensions)
		if (fileName.EndsWith($".{extension}", StringComparison.CurrentCultureIgnoreCase))
			return true;
	// Si ha llegado hasta aquí es porque no se corresponde
	return false;
}

// Prepara el contenido de un archivo de imagen
string PrepareImageFile(string fileResult)
{
	return $"""
			#begin {fileResult}
			/importer:TextureImporter
			/processor:TextureProcessor
			/processorParam:ColorKeyColor=255,0,255,255
			/processorParam:ColorKeyEnabled=True
			/processorParam:GenerateMipmaps=False
			/processorParam:PremultiplyAlpha=True
			/processorParam:ResizeToPowerOfTwo=False
			/processorParam:MakeSquare=False
			/processorParam:TextureFormat=Color
			/build:{fileResult}

			""";
}

// Prepara el contenido de copia de archivo (de texto, por ejemplo)
string PrepareCopyFile(string fileResult)
{
	return $"""
			#begin {fileResult}
			/copy:{fileResult}

			""";
}

// Prepara el contenido de copia de archivo de efectos de sonido (WAV)
string PrepareSoundEffectWavFile(string fileResult)
{
	return $"""
			#begin {fileResult}
			/importer:WavImporter
			/processor:SoundEffectProcessor
			/processorParam:Quality=Best
			/build:{fileResult}

			""";
}

// Prepara el contenido de copia de archivo de canción (MP3)
string PrepareSongMp3File(string fileResult)
{
	return $"""
			#begin {fileResult}
			/importer:Mp3Importer
			/processor:SongProcessor
			/processorParam:Quality=Best
			/build:{fileResult}

			""";
}

// Prepara el contenido de copia de archivo de fuentes (SpriteFont)
string PrepareSpriteFontFile(string fileResult)
{
	return $"""
			#begin {fileResult}
			/importer:FontDescriptionImporter
			/processor:FontDescriptionProcessor
			/processorParam:PremultiplyAlpha=True
			/processorParam:TextureFormat=Compressed
			/build:{fileResult}

			""";
}

// Prepara el contenido de shader
string PrepareShader(string fileResult)
{
	return $"""
			#begin {fileResult}
			/importer:EffectImporter
			/processor:EffectProcessor
			/processorParam:DebugMode=Auto
			/build:{fileResult}
			
			""";
}