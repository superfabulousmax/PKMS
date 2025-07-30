from pydantic import BaseModel


class NoteBase(BaseModel):
    title: str
    body: str


class NoteCreate(NoteBase):
    pass


class NoteRead(NoteBase):
    id: int

    model_config = {
        "from_attributes": True
    }