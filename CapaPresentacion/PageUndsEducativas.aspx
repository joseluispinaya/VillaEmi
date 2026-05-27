<%@ Page Title="" Language="C#" MasterPageFile="~/HomePage.Master" AutoEventWireup="true" CodeBehind="PageUndsEducativas.aspx.cs" Inherits="CapaPresentacion.PageUndsEducativas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>

        /*.table thead th {
            border-top: 0;
            font-size: 13px;
            font-weight: 600;
            white-space: nowrap;
        }*/

        .icon-unidad {
            width: 45px;
            height: 45px;
            border-radius: 12px;
            background: rgba(0,123,255,.1);
            display: flex;
            align-items: center;
            justify-content: center;
            color: #007bff;
            font-size: 20px;
        }

        /*.table td {
            vertical-align: middle !important;
        }*/

        .btn-action {
            width: 34px;
            height: 34px;
            padding: 0;
            border-radius: 8px;
        }

        /*.dataTables_wrapper .dataTables_filter input {
            border-radius: 8px;
            border: 1px solid #ced4da;
            padding: 5px 10px;
        }

        .dataTables_wrapper .dataTables_length select {
            border-radius: 8px;
        }

        .table-hover tbody tr:hover {
            background: #f8fbff;
        }*/
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="row">
        <div class="col-md-12">
            <div class="card card-primary">
                <div class="card-header d-flex justify-content-between align-items-center">
                    <h3 class="card-title">
                        <i class="fas fa-school"></i>
                        Panel de Unidades Educativas
                    </h3>
                    <button type="button" id="btnRegistro" class="btn btn-success btn-sm"><i class="fas fa-school mr-2"></i>Nuevo Registro</button>
                </div>
                <div class="card-body">
                    <div class="row">
                        <div class="col-12">
                            <table class="table table-bordered table-striped table-sm align-middle" id="tbDatas" style="width: 100%">
                                <thead>
                                    <tr>
                                        <th>Id</th>
                                        <th>Unidad Educativa</th>
                                        <th>Información</th>
                                        <th>Acciones</th>
                                    </tr>
                                </thead>
                                <tbody>
                                </tbody>
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
                    <h4 id="myModalLabel" class="modal-title">Registro Unidad Educativa</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-sm-6">
                            <div class="form-group">
                                <label for="txtnombres">Nombre U.E.</label>
                                <input type="text" class="form-control form-control-sm model" id="txtnombres" name="Nombres" placeholder="Nombre U.E.">
                            </div>
                        </div>
                        <div class="col-sm-6">

                            <div class="form-group">
                                <label for="txtNroCel">Celular:</label>
                                <input type="text" class="form-control form-control-sm model" id="txtNroCel" name="NroCel" placeholder="Celular">
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="txtUbicacion">Ubicacion</label>
                        <input type="text" class="form-control form-control-sm model" id="txtUbicacion" name="Ubicacion">
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
    <script src="js/UnidsEducativas.js?v=<%= DateTime.Now.ToString("yyyyMMddHHmmss") %>" type="text/javascript"></script>
</asp:Content>
