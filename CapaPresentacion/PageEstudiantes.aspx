<%@ Page Title="" Language="C#" MasterPageFile="~/HomePage.Master" AutoEventWireup="true" CodeBehind="PageEstudiantes.aspx.cs" Inherits="CapaPresentacion.PageEstudiantes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="row">
    <div class="col-lg-6">
        <div class="card">
            <div class="card-header bg-primary py-2 px-4">
                <h3 class="card-title m-0"><i class="fas fa-user-friends mr-2"></i>Lista de Estudiantes</h3>
            </div>
            <div class="card-body">

                <div class="form-row align-items-end mb-3">

                    <div class="form-group col-sm-8">
                        <label for="cboUnidadEdGe">Seleccione UE Educativa</label>
                        <select class="form-control form-control-sm" id="cboUnidadEdGe">
                        </select>
                    </div>

                    <div class="form-group col-sm-4">
                        <button type="button" id="btnNuevo" class="btn btn-primary btn-sm"><i class="fas fa-plus-circle mr-2"></i>Nuevo Registro</button>
                    </div>
                </div>

                <div class="row">
                    <div class="col-lg-12 col-sm-12 col-12">
                        <table class="table table-sm table-striped table-bordered" id="tbData" style="width: 100%">
                            <thead>
                                <tr>
                                    <th>Id</th>
                                    <th>Estudiantes</th>
                                    <%--<th>Nro CI</th>
                                    <th>Estado</th>--%>
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
    <div class="col-lg-6">
        <div class="card">
            <div class="card-header bg-primary py-2 px-4">
                <h3 class="card-title m-0"><i class="fas fa-user-friends mr-2"></i>Historial Test</h3>
            </div>
            <div class="card-body">
                <h5 class="mb-3 mt-0 text-center" id="lblNombreEsr">Lista</h5>

                <div class="row">
                    <div class="col-lg-12 col-sm-12 col-12">
                        <table class="table table-sm table-striped table-bordered" id="tbDataHist" style="width: 100%">
                            <thead>
                                <tr>
                                    <th>Id</th>
                                    <th>Descripcion General</th>
                                    <th></th>
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


<div class="modal fade" id="mdDetalles" aria-hidden="true">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-header">
                <h4 id="myModalLabeldett" class="modal-title">Detalle</h4>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <h5 class="mb-3 mt-0 text-center">Lista de Recomendaciones</h5>
                <div class="row">
                    <div class="col-sm-12">
                        <table class="table table-sm table-striped table-bordered" id="tbDetalleHistor" style="width: 100%">
                            <thead>
                                <tr>
                                    <th>Carrera</th>
                                    <th>Justificacion</th>
                                    <th>Puntaje</th>
                                </tr>
                            </thead>
                            <tbody></tbody>
                        </table>
                    </div>
                </div>
            </div>
            <div class="modal-footer justify-content-between">
                <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                <button type="button" class="btn btn-primary" id="btnReport"><i class="fas fa-file-pdf mr-2"></i>Generar Reporte</button>
            </div>
        </div>
        <!-- /.modal-content -->
    </div>
    <!-- /.modal-dialog -->
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
    <script src="js/Estudiantes.js?v=<%= DateTime.Now.ToString("yyyyMMddHHmmss") %>" type="text/javascript"></script>

</asp:Content>
