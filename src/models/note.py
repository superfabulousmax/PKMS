from sqlalchemy import Integer, String, Text, ForeignKey, Table, Column
from sqlalchemy.orm import Mapped, mapped_column, relationship
from src.models.base import Base

note_links = Table(
    "note_links",
    Base.metadata,
    Column("from_note_id", ForeignKey("notes.id"), primary_key=True),
    Column("to_note_id", ForeignKey("notes.id"), primary_key=True),
)


class Note(Base):
    __tablename__ = "notes"

    id: Mapped[int] = mapped_column(Integer, primary_key=True, index=True)
    title: Mapped[str] = mapped_column(String(100), nullable=False)
    body: Mapped[str] = mapped_column(Text, nullable=True)

    links: Mapped[list["Note"]] = relationship(
        "Note",
        secondary=note_links,
        primaryjoin=id == note_links.c.from_note_id,
        secondaryjoin=id == note_links.c.to_note_id,
        backref="linked_by"
    )