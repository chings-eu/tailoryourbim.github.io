require.config({ paths: { 'vs': 'https://cdnjs.cloudflare.com/ajax/libs/monaco-editor/0.45.0/min/vs' }});
window.MonacoEnvironment = { getWorkerUrl: () => proxy };

let proxy = URL.createObjectURL(new Blob([`
	self.MonacoEnvironment = {
		baseUrl: 'https://cdnjs.cloudflare.com/ajax/libs/monaco-editor/0.45.0/min/'
	};
	importScripts('https://cdnjs.cloudflare.com/ajax/libs/monaco-editor/0.45.0/min/vs/base/worker/workerMain.min.js');
`], { type: 'text/javascript' }));

require(["vs/editor/editor.main"], function () {
    
	window.editor = monaco.editor.create(document.getElementById('editor'), {
		value: [
			``
		].join('\n'),
		language: 'javascript',
		theme: 'vs-light'
	});
});

