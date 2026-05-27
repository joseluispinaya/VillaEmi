<%@ Page Title="" Language="C#" MasterPageFile="~/HomePage.Master" AutoEventWireup="true" CodeBehind="PageDirectores.aspx.cs" Inherits="CapaPresentacion.PageDirectores" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="dist/css/inpfile.css" rel="stylesheet"/>
    <style>
        .est-perfil {
            width: 125px;
            height: 125px;
            object-fit: cover; /* Evita que la imagen se estire o aplaste */
            object-position: center; /* Asegura que se vea el centro de la foto */
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="row">
    <div class="col-lg-12">
        <div class="card">
            <div class="card-header bg-primary py-2 px-4">
                <h3 class="card-title m-0"><i class="fas fa-school mr-2"></i>Lista Directores Registrados</h3>
            </div>
            <div class="card-body">
                <div class="row justify-content-center mb-4">
                    <button type="button" id="btnRegistro" class="btn btn-success btn-sm"><i class="fas fa-user-plus mr-2"></i>Nuevo Registro</button>
                </div>


                <div class="row mt-3">
                    <div class="col-lg-12 col-sm-12 col-12">
                        <table id="tbDatas" class="table table-sm table-striped table-bordered" style="width: 100%">
                            <thead>
                                <tr>
                                    <th>Id</th>
                                    <th>Nombre Directores</th>
                                    <th>Correo</th>
                                    <th>Nombre U.E.</th>
                                    <th>Estado</th>
                                    <th>Acciones</th>
                                </tr>
                            </thead>
                            <tbody></tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>


<div class="modal fade" id="mdData" aria-hidden="true">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-header">
                <h4 id="myModalLabel" class="modal-title">Detalle</h4>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <div class="row">
                    <div class="col-lg-9">
                        <div class="form-row">
                            <div class="form-group col-sm-4">
                                <label for="txtNombres">Nombre</label>
                                <input type="text" class="form-control form-control-sm model" id="txtNombres" name="Nombres">
                            </div>
                            <div class="form-group col-sm-4">
                                <label for="txtApellidos">Apellidos</label>
                                <input type="text" class="form-control form-control-sm model" id="txtApellidos" name="Apellidos">
                            </div>
                            <div class="form-group col-sm-4">
                                <label for="txtCorreo">Correo</label>
                                <input type="text" class="form-control form-control-sm model" id="txtCorreo" name="Correo">
                            </div>
                        </div>
                        <div class="form-row">
                            <div class="form-group col-sm-3">
                                <label for="txtCelular">Celular</label>
                                <input type="text" class="form-control form-control-sm model" id="txtCelular" name="Celular">
                            </div>
                            <div class="form-group col-sm-3">
                                <label for="txtNroci">Nro CI</label>
                                <input type="text" class="form-control form-control-sm model" id="txtNroci" name="Nro ci">
                            </div>
                            <div class="form-group col-sm-6">
                                <label for="cboUnidadEd">Unidad Ed</label>
                                <select class="form-control form-control-sm" id="cboUnidadEd">
                                </select>
                            </div>

                        </div>
                        <div class="form-row">
                            <div class="form-group col-sm-3">
                                <label for="cboEstado">Estado</label>
                                <select class="form-control" id="cboEstado">
                                    <option value="1">Activo</option>
                                    <option value="0">No Activo</option>
                                </select>
                            </div>
                            <div class="form-group col-sm-9">
                                <label for="txtApelocs">Seleccione Foto</label>
                                <div class="custom-file-upload">
                                    <input type="file" class="custom-file-input-upload" id="txtFotoUr">
                                    <label class="custom-file-label-upload" for="txtFotoUr">Ningún archivo seleccionado</label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-3">
                        <div class="form-row h-100 d-flex align-items-center justify-content-center">
                            <div class="form-group col-sm-12 text-center">
                                <img id="imgDirectReg" src="images/sinimagen.png" alt="Foto usuario" class="est-perfil">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal-footer justify-content-between">
                <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                <button type="button" class="btn btn-primary" id="btnGuardarCambios">Guardar</button>
            </div>
        </div>
        <!-- /.modal-content -->
    </div>
    <!-- /.modal-dialog -->
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
    <script src="js/PageDirectores.js?v=<%= DateTime.Now.ToString("yyyyMMddHHmmss") %>" type="text/javascript"></script>

</asp:Content>
