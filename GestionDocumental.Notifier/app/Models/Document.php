<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Model;
use Illuminate\Database\Eloquent\Factories\HasFactory;

class Document extends Model
{
    //
    use HasFactory;

    protected $table = 'Documentos';

    protected $primaryKey = 'ID';

    public $timestamps = false;
    public $incrementing = false;

    protected $keyType = 'string';

    protected $fillable = [
        'Estado',
        'FechaValidado'
    ];
}
