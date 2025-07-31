from pydantic import BaseModel
from typing import Optional


class NoteBase(BaseModel):
    title: str
    body: Optional[str]


class NoteCreate(NoteBase):
    pass


class NoteRead(NoteBase):
    id: int

    model_config = {
        "from_attributes": True
    }