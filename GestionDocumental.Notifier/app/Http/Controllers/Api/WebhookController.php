<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\Controller;
use App\Models\Document;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Validator;

class WebhookController extends Controller
{
    /**
     * Actualiza el estado de un documento a 'Validado'.
     */
    public function validateDocument(Request $request)
    {
        // 1. Validar los datos recibidos
        $validator = Validator::make($request->all(), [
            'documentoId' => 'required|string|uuid', // Requerimos un ID de documento GUID
            'nuevoEstado' => 'required|string|in:Validado', // Requerimos que el estado sea exactamente "Validado"
        ]);

        if ($validator->fails()) {
            return response()->json([
                'status' => 'error',
                'message' => 'Datos inválidos',
                'errors' => $validator->errors(),
            ], 400); // 400 Bad Request
        }

        // 2. Buscar el documento en la base de datos
        $document = Document::find($request->input('documentoId'));

        if (!$document) {
            return response()->json([
                'status' => 'error',
                'message' => 'Documento no encontrado',
            ], 404); // 404 Not Found
        }

        // 3. Actualizar el estado y la fecha de validación
        $document->Estado = $request->input('nuevoEstado');
        $document->FechaValidado = now();
        $document->save(); // Guarda los cambios en la BD

        // 4. Retornar una respuesta de éxito
        return response()->json([
            'status' => 'success',
            'message' => 'El estado del documento ha sido actualizado a Validado.',
            'documento_id' => $document->ID,
        ]);
    }
}