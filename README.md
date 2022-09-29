# console_fbevent

Neste demo escrita em .NET6 está um exemplo simples(console) para receber notificação de eventos a partir do disparo de uma Trigger no firebird.

A base de dados que acompanha este demo foi criada com Firebird 2.5

Nesta base há duas tabelas:  USUARIO e SINC_USER

Para cada evento INSERT ou UPDATE na tabela USUARIO uma trigger(TR_USUARIO) será acionada e gavará o ID inserido ou alterado na tabela SINC_USER.

Então é apartir da trigger TR_USUARIO que o Firebird emitirá o evento.

Este demo inicia uma aplicação console que fica escutando o evento "SINC_USUARIO" e quando o recebe, escreve um log na tela.

Numa aplicação real o programador, ao receber a notificação do evento, escolherá qual rotina deve ser executada.

Abaixo está a trigger criada:


CREATE OR ALTER TRIGGER TR_USUARIO FOR USUARIO
ACTIVE AFTER INSERT OR UPDATE POSITION 0
AS
begin
merge into SINC_USER s
  using USUARIO u
  on new.ID_USUARIO = s.ID_USUARIO
  when matched then
    UPDATE SET ID_USUARIO = new.ID_USUARIO, SINCRONIZADO = 'N', TIPO = 'U'
  when not matched then
    INSERT (ID, ID_USUARIO, SINCRONIZADO, TIPO)
    VALUES (gen_id(gen_sinc_user_id, 1), new.ID_USUARIO, 'N', 'I');

    POST_EVENT 'SINC_USUARIO';
end
