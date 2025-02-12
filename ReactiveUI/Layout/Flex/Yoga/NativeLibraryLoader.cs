using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Reactive.Yoga {
    internal static class NativeLibraryLoader {
        private static string? _libraryPath;
        private static bool _isInitialized;

        public static string LibraryPath => _libraryPath ?? throw new InvalidOperationException("Library not initialized");

        public static string Initialize() {
            
            var assemblyName = typeof(NativeLibraryLoader).Assembly.GetName().Name;
            var resourceName = $"{assemblyName}.yoga.dll";
            
            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream(resourceName);
            
            if (stream == null) {
                throw new InvalidOperationException($"Could not find embedded resource: {resourceName}");
            }

            var tempDirectory = Path.Combine(Path.GetTempPath(), "Reactive.BeatSaber", "native");
            Directory.CreateDirectory(tempDirectory);

            _libraryPath = Path.Combine(tempDirectory, "yoga.dll");
            
            // Check if we need to extract the DLL
            var shouldExtract = true;
            if (File.Exists(_libraryPath)) {
                try {
                    // Try to load the existing DLL to see if it's valid
                    var handle = LoadLibrary(_libraryPath);
                    if (handle != IntPtr.Zero) {
                        FreeLibrary(handle);
                        shouldExtract = false;
                    }
                }
                catch {
                    // If loading fails, we'll extract a new copy
                    shouldExtract = true;
                }
            }

            if (shouldExtract) {
                // Extract the DLL
                using var fileStream = File.Create(_libraryPath);
                stream.CopyTo(fileStream);
            }

            // Load the DLL
            if (LoadLibrary(_libraryPath) == IntPtr.Zero) {
                var error = Marshal.GetLastWin32Error();
                throw new InvalidOperationException($"Failed to load native library: {_libraryPath}, Error: {error}");
            }

            _isInitialized = true;

            return _libraryPath;
        }

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32", SetLastError = true)]
        private static extern bool FreeLibrary(IntPtr hModule);
    }
} 