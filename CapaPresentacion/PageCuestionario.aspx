<%@ Page Title="" Language="C#" MasterPageFile="~/HomePage.Master" AutoEventWireup="true" CodeBehind="PageCuestionario.aspx.cs" Inherits="CapaPresentacion.PageCuestionario" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="row">
    <div class="col-lg-12">
        <div class="card">
            <div class="card-header bg-primary py-2 px-4">
                <h3 class="card-title m-0"><i class="fas fa-school mr-2"></i>Lista Cuestionarios Registrados</h3>
            </div>
            <div class="card-body">
                <div class="row justify-content-center mb-4">
                    <button type="button" id="btnRegistro" class="btn btn-primary btn-sm"><i class="fas fa-user-plus mr-2"></i>Nuevo Registro</button>
                </div>


                <div class="row mt-3">
                    <div class="col-lg-12 col-sm-12 col-12">
                        <table id="tbDatas" class="table table-bordered table-striped table-sm" style="width: 100%">
                            <thead>
                                <tr>
                                    <th>Id</th>
                                    <th>Titulo</th>
                                    <th>Descripcion</th>
                                    <th>Preguntas</th>
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
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <h4 id="myModalLabel" class="modal-title">Detalle</h4>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <div class="form-group">
                    <label for="txtTitulo">Titulo</label>
                    <input type="text" class="form-control" id="txtTitulo" autocomplete="off">
                </div>
                <div class="form-group">
                    <label for="txtDescripcion">Descripcion</label>
                    <textarea class="form-control" rows="3" id="txtDescripcion"></textarea>
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
    <script src="js/PageCuestionario.js?v=<%= DateTime.Now.ToString("yyyyMMddHHmmss") %>" type="text/javascript"></script>
</asp:Content>
